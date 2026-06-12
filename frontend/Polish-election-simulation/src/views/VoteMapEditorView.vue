<template>
  <LoadingComponent v-if="isLoading" spinner-color="lightgreen" />
  <div v-else>
    <div class="container">
      <div class="top-row">
        <div class="title-section">
          <h1>Votes & map editor</h1>
        </div>
        <div class="controls">
          <Select
              v-model="simDataUUID"
              :options="simDataList"
              optionLabel="name"
              optionValue="id"
              placeholder="Select data"
              @change="loadSimData"
          />
          <Button label="New" icon="pi pi-plus" severity="secondary" @click="showNewDataDialog = true"/>
          <Button label="Save" icon="pi pi-save" :loading="saving" :disabled="!simDataUUID" @click="updateSimData"/>
        </div>
        <Dialog v-model:visible="showNewDataDialog" header="New dataset" modal :style="{ width: '25rem' }">
          <div class="dialog-body">
            <label for="dataset-name">Dataset name</label>
            <InputText id="dataset-name" v-model="newDataName" placeholder="Enter dataset name" autofocus />
          </div>
          <template #footer>
            <Button label="Cancel" severity="secondary" @click="showNewDataDialog = false" />
            <Button label="Create" icon="pi pi-check" :loading="creating" @click="createSimData" />
          </template>
        </Dialog>
      </div>
      <div class="map-palette-row">
        <div class="map">
          <ConstituencyEditorMap
              :constituencies="powiats"
              :fillMap="fillMap"
              :highlightedPowiats="selectedGroup?.powiats!"
              @feature-click="handleFeatureClick"
          />
        </div>
        <div class="constituency-creator">
          <div class="palette-toolbar">
            <Button
                :label="eraserActive ? 'Erasing' : 'Erase'"
                icon="pi pi-eraser"
                :outlined="!eraserActive"
                :severity="eraserActive ? 'danger' : 'secondary'"
                :class="{ active: eraserActive }"
                @click="toggleEraser"
                size="small"
                title="Eraser"
            />
            <Button
                label="Add constituency"
                icon="pi pi-plus"
                outlined
                severity="secondary"
                size="small"
                @click="addGroupToView"
                :disabled="visibleGroupIds.size >= 100"
                title="Add constituency"
            >Add constituency</Button>
            <InputNumber
                class="seat-setter"
                v-if="selectedGroupId !== null"
                v-model="selectedGroup!.seats"
                :min="0"
                placeholder="Seats"
                size="small"
            />
          </div>
          <div class="palette">
            <div
                v-for="group in visibleGroups"
                :key="group.id"
                class="constituency-circle"
                :class="{ selected: selectedGroupId === group.id && !eraserActive }"
                @click="selectGroup(group.id)"
            >
              <svg :width="20" :height="20" xmlns="http://www.w3.org/2000/svg">
                <circle
                    :cx="10"
                    :cy="10"
                    :r="10"
                    :fill="group.color"
                />
              </svg>
              <button
                  class="remove-btn"
                  @click.stop="removeGroupFromView(group.id)"
              >×</button>
            </div>
          </div>
        </div>
        <div class="party-creator">
          <table>
            <thead>
            <tr>
              <th>Party name</th>
              <th>Needs threshold</th>
              <th>Coalition</th>
              <th></th>
            </tr>
            </thead>
            <tbody>
            <tr v-for="column in columns" :key="column.id" class="column-pill">
              <td>
                <InputText
                    v-model="column.name"
                    :class="{ 'p-invalid': column.name.trim() === '' }"
                    size="small"
                    @blur="validateName(column)"
                    @keydown.enter="($event.target as HTMLInputElement).blur()"
                    @keydown.esc="revert(column, $event)"
                />
              </td>
              <td class="center-cell">
                <input
                    type="checkbox"
                    v-model="column.needsThreshold"
                />
              </td>
              <td class="center-cell">
                <input
                    type="checkbox"
                    v-model="column.isCoalition"
                />
              </td>
              <td class="center-cell">
                <Button
                    icon="pi pi-times"
                    text
                    rounded
                    severity="danger"
                    size="small"
                    @click="removeColumn(column.id)"
                />
              </td>
            </tr>
            </tbody>
          </table>
          <Button
              class="add-btn"
              label="Add party"
              icon="pi pi-plus"
              outlined
              severity="secondary"
              size="small"
              @click="addColumn"
          />
        </div>
      </div>
      <div class="data-table-wrapper">
        <table class="p-datatable-table data-table">
          <thead>
          <tr>
            <th>Powiat</th>
            <th v-for="col in columns" :key="col.id">{{ col.name }}</th>
          </tr>
          </thead>
          <tbody>
          <tr v-for="row in rows" :key="row.name" :style="{ background: getRowColor(row.terc) ?? '' }">
            <td>{{ row.name }}</td>
            <td v-for="col in columns" :key="col.id">
              <input
                  class="votes-input"
                  type="number"
                  :value="row[`party${col.id}`]"
                  @change="row[`party${col.id}`] = Number(($event.target as HTMLInputElement).value)"
              />
            </td>
          </tr>
          </tbody>
        </table>
      </div>

    </div>
  </div>
</template>

<script setup lang="ts">
import {computed, nextTick, onMounted, ref} from "vue";

import Button from "primevue/button";
import InputText from "primevue/inputtext";
import Select from "primevue/select";
import InputNumber from "primevue/inputnumber";
import { useToast } from "primevue/usetoast";
import Dialog from "primevue/dialog"

import LoadingComponent from "@/components/LoadingComponent.vue";
import ConstituencyEditorMap from "@/components/ConstituencyEditorMap.vue";
import { loadPowiaty } from "@/api/constituencyLoader.ts";
import { loadColors } from "@/api/colorLoader.ts";
import apiClient from "@/api/apiClient.ts";
import VotesID from "@/api/VotesID.ts";
import type {SimulationData} from "@/api/SimulationData.ts";

const toast = useToast();

const saving = ref(false);
const creating = ref(false);
const showNewDataDialog = ref(false);
const newDataName = ref("");

const isLoading = ref(true);
const powiats = ref<any>(null);

interface Column {
  id: number,
  name: string,
  prev: string,
  isCoalition: boolean,
  needsThreshold: boolean
}

const columns = ref<Column[]>([
  { id: 1, name: "Party 1", prev: "Party 1", isCoalition: false, needsThreshold: false},
  { id: 2, name: "Party 2", prev: "Party 2", isCoalition: false, needsThreshold: false}
]);
let idCounter = columns.value.length + 1;


interface Row {
  terc: string,
  name: string,
  [key: string]: any
}
const rows = ref<Row[]>([]);

interface RowGroup {
  id: number,
  color: string,
  seats: number,
  powiats: Set<string>
}
const rowGroups = ref<RowGroup[]>([]);

const selectedGroupId = ref<number | null>(null);
const selectedGroup = computed(() =>
    rowGroups.value.find(g => g.id === selectedGroupId.value)
);
const eraserActive = ref(false);
const visibleGroupIds = ref<Set<number>>(new Set());
const visibleGroups = computed(() =>
    rowGroups.value.filter(g => visibleGroupIds.value.has(g.id))
);

const simDataList = ref<VotesID[]>([]);
const simDataUUID = ref<string>("");

onMounted(async () => {
  powiats.value = await loadPowiaty();
  simDataList.value = await apiClient.getVotesIDs();
  for (const powiat of powiats.value.features) {
    rows.value.push(
        { terc: powiat.properties.terc, name: powiat.properties.name, party1: 0, party2: 0 }
    );
  }
  const colors = await loadColors();
  let i = 1;
  for (const color of colors) {
    rowGroups.value.push(
        { id: i++, color, seats: 0, powiats: new Set() }
    );
  }
  visibleGroupIds.value = new Set(rowGroups.value.slice(0, 5).map(g => g.id));
  isLoading.value = false;
});

function isDuplicateColumnName(id: number, name: string) {
  return columns.value.filter(c => c.id != id).map(c => c.name === name).reduce((a, b) => a || b, false);
}

function validateName(column: Column) {
  if (column.name.trim() === '' || isDuplicateColumnName(column.id, column.name)) {
    column.name = column.prev;
  }
  else {
    column.prev = column.name;
  }
}

function revert(column: Column, event: KeyboardEvent) {
  column.name = column.prev;
  (event.target as HTMLInputElement).blur();
}

function removeColumn(id: number) {
  columns.value = columns.value.filter(c => c.id !== id);
  const field = `party${id}`;
  rows.value = rows.value.map(row => {
    const { [field]: _, ...rest } = row;
    return rest;
  }) as Row[];
}

async function addColumn() {
  const id = idCounter++;
  const name = `Party ${id}`;
  columns.value.push({ id, name, prev: name, isCoalition: false, needsThreshold: false});
  const field = `party${id}`;
  rows.value = rows.value.map(row => ({ ...row, [field]: 0 }));

  await nextTick();
  const inputs = document.querySelectorAll<HTMLInputElement>('.column-pill input');
  inputs[inputs.length - 1]?.select();
}

function selectGroup(id: number) {
  eraserActive.value = false;
  selectedGroupId.value = selectedGroupId.value === id ? null : id;
}

function toggleEraser() {
  eraserActive.value = !eraserActive.value;
  if (eraserActive.value) selectedGroupId.value = null;
}

function addGroupToView() {
  const next = rowGroups.value.find(g => !visibleGroupIds.value.has(g.id));
  if (next) visibleGroupIds.value = new Set([...visibleGroupIds.value, next.id]);
}

function removeGroupFromView(id: number) {
  if (selectedGroupId.value === id) selectedGroupId.value = null;
  const group = rowGroups.value.find(g => g.id === id);
  if (group) group.powiats.clear();
  visibleGroupIds.value = new Set([...visibleGroupIds.value].filter(i => i !== id));
}

function getRowColor(terc: string): string | null {
  return rowGroups.value.find(g => g.powiats.has(terc))?.color ?? null;
}

const fillMap = computed(() => {
  const map: Record<string, string> = {};
  for (const group of rowGroups.value) {
    for (const terc of group.powiats) {
      map[terc] = group.color;
    }
  }
  return map;
})

function handleFeatureClick(terc: string) {
  if (eraserActive.value) {
    for (const group of rowGroups.value) {
      group.powiats.delete(terc);
    }
    return;
  }
  if (selectedGroupId.value === null) return;

  const group = rowGroups.value.find(g => g.id === selectedGroupId.value);
  if (!group) return;

  for (const g of rowGroups.value) {
    if (g.id !== group.id) g.powiats.delete(terc);
  }
  group.powiats.add(terc);
}

async function loadSimData() {
  const data: SimulationData = await apiClient.getSimulationData(simDataUUID.value);
  let new_columns: Column[] = [];
  for (const group of rowGroups.value) group.powiats.clear();
  visibleGroupIds.value = new Set();
  for (const [index, party] of data.parties.entries()) {
    new_columns.push({
      id: index + 1,
      name: party.name,
      prev: party.name,
      isCoalition: party.isCoalition,
      needsThreshold: party.needsThreshold
    });
  }
  columns.value = new_columns;
  idCounter = new_columns.length + 1;

  visibleGroupIds.value = new Set();
  const forbiddenPowiats = ["SPEC", "1498", "1499"];
  for(const [index, district] of Object.entries(data.districts)) {
    if(district.terytCodes.includes("SPEC")) district.terytCodes = district.terytCodes.filter(str => !forbiddenPowiats.includes(str));
    const id_num = Number(index);
    rowGroups.value[id_num - 1]!.powiats = new Set(district.terytCodes);
    rowGroups.value[id_num - 1]!.seats = district.seats;
    visibleGroupIds.value.add(id_num);
  }

  const districtByTerc = new Map<string, string>();
  for(const [districtId, district] of Object.entries(data.districts)) {
    for(const terc of district.terytCodes) {
      districtByTerc.set(terc, districtId);
    }
  }

  rows.value = rows.value.map(row => {
    const votes = data.votesInAreas[row.terc]!;
    const updated: Row = {
      terc: row.terc,
      name: row.name
    };
    votes.forEach((vote, index) => {
      updated[`party${index + 1}`] = vote;
    });
    return updated;
  });
}

function validateSimData(data: SimulationData): string[] {
  const errors: string[] = [];

  for (const [id, district] of Object.entries(data.districts)) {
    if (!district.terytCodes || district.terytCodes.length === 0) {
      errors.push(`District ${id} has no powiats assigned.`);
    }
  }

  const assignedTercs = new Set(
      Object.values(data.districts).flatMap(d => d.terytCodes)
  );
  const unassigned = rows.value
      .filter(row => !assignedTercs.has(row.terc))
      .map(row => row.name);

  if (unassigned.length > 0) {
    errors.push(
        `${unassigned.length} powiat(s) not assigned to any district: ${unassigned.slice(0, 5).join(', ')}${unassigned.length > 5 ? '…' : ''}.`
    );
  }

  return errors;
}


async function gatherSimData() {
  let result: SimulationData = {
    parties: columns.value.map(col => ({
      name: col.name,
      isCoalition: col.isCoalition,
      needsThreshold: col.needsThreshold,
    })),
    districts: {},
    votesInAreas: {}
  };

  for (const group of visibleGroups.value) {
    result.districts[group.id.toString()] = {
      seats: group.seats,
      terytCodes: [...group.powiats],
    };
  }

  for (const row of rows.value) {
    result.votesInAreas[row.terc] = columns.value.map(
        col => row[`party${col.id}`]
    )
  }
  return result;
}

async function updateSimData() {
  saving.value = true;
  try {
    const data = await gatherSimData();
    const errors = validateSimData(data);
    if (errors.length > 0) {
      errors.forEach(err => toast.add({ severity: "error", summary: "Validation error", detail: err, life: 6000}));
      return;
    }
    await apiClient.sendSimulationData(data, simDataUUID.value);
    toast.add({ severity: "success", summary: "Saved", detail: "Dataset updated successfully", life: 3000});
  } catch (err) {
    let detail = String(err instanceof Error ? err.message : err);
    try {
      const parsed = JSON.parse(detail);
      if (parsed.detail) detail = parsed.detail;
    } catch { /* not JSON */ }
    toast.add({ severity: "error", summary: "Save failed", detail, life: 6000 });
  } finally {
    saving.value = false;
  }
}

async function createSimData() {
  const label = newDataName.value.trim();
  if (!label) return;
  creating.value = true;
  try {
    const data = await gatherSimData();
    const errors = validateSimData(data);
    if (errors.length > 0) {
      errors.forEach(err => toast.add({ severity: "error", summary: "Validation error", detail: err, life: 6000}));
      return;
    }
    const created = await apiClient.sendSimulationData(data, undefined, label);
    simDataList.value = await apiClient.getVotesIDs();
    simDataUUID.value = created.id;
    showNewDataDialog.value = false;
    newDataName.value = "";
    toast.add({ severity: "success", summary: "Created", detail: "New dataset created successfully", life: 3000});
  } catch (err) {
    let detail = String(err instanceof Error ? err.message : err);
    try {
      const parsed = JSON.parse(detail);
      if (parsed.detail) detail = parsed.detail;
    } catch { /* not JSON */ }
    toast.add({ severity: "error", summary: "Creating failed", detail, life: 6000 });
  } finally {
    creating.value = false;
  }
}


</script>

<style>
@media (min-width: 1024px) {
  .about {
    min-height: 100vh;
    display: flex;
    align-items: center;
    width: 100%;
  }
}

.container {
  display: flex;
  flex-direction: column;
  width: 100%;
  margin: 0 auto;
  /* no fixed height, let content determine it */
}

.top-row {
  flex-shrink: 0;
  display: flex;
  justify-content: space-between;
  gap: 10px;
  padding: 2rem;
}

.title-section {
  flex: 1;
  text-align: center;
}

.controls {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
  align-items: center;
}

.constituency-creator {
  flex: 1 0 300px;
  height: 100%;
  display: flex;
  flex-direction: column;        /* stack toolbar above palette */
  align-items: flex-start;
  justify-content: center;
  gap: 10px;
  padding: 12px;
  overflow: hidden;
}

.map-palette-row {
  display: flex;
  flex-wrap: nowrap;
  min-height: 0;
  overflow-x: auto;
}

.palette-toolbar {
  display: flex;
  flex-direction: row;
  gap: 8px;
  flex-shrink: 0;
}

.palette {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  align-content: flex-start;
  max-width: calc(8 * (32px)); /* up to 8 circles per row */
  overflow-y: auto;
}

.constituency-circle {
  position: relative;
  width: 32px;
  height: 32px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
  cursor: pointer;
  border: 2px solid transparent;
  transition: border-color 0.15s;
}

.constituency-circle.selected {
  box-shadow: 0 0 0 2px;
}

.remove-btn {
  position: absolute;
  top: -4px;
  right: -4px;
  width: 14px;
  height: 14px;
  border-radius: 50%;
  border: none;
  font-size: 10px;
  line-height: 1;
  cursor: pointer;
  display: none;
  align-items: center;
  justify-content: center;
  padding: 0;
}

.constituency-circle:hover .remove-btn {
  display: flex;
}

.p-datatable-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 14px;
}

.data-table-wrapper {
  overflow-x: auto;
  margin-top: 1.5rem;
}

.data-table {
  min-width: max-content;
  margin-top: 1.5rem;
}

.p-datatable-table th {
  padding: 10px 12px;
  text-align: left;
  font-weight: 600;
  font-size: 13px;
  border-bottom: 1px solid;
  max-width: 100px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.p-datatable-table td {
  padding: 6px 12px;
  border-bottom: 1px solid;
  vertical-align: middle;
}

.center-cell {
  text-align: center;
}

.seat-setter {
  max-width: 20px !important;
}

.party-creator {
  flex: 1 0 420px;
  height: 100%;
  min-width: 0;
  display: flex;
  flex-direction: column;
  gap: 10px;
  padding: 12px;
  overflow: auto;
}

.add-btn {
  align-self: flex-start;
  margin-top: 4px;
  line-height: normal;
}

.votes-input {
  width: 80px !important;
}

/* ensure a sensible row height when items are side-by-side */
@media (min-width: 769px) {
  .map-palette-row {
    height: 50vh;
  }
}

.map {
  flex: 1 0 300px;
  height: 100%;
  overflow: hidden;
}

@media (max-width: 768px) {
  .map-palette-row {
    flex-direction: column;
    flex: 0 0 auto; /* size to children */
    min-height: 0;
    height: auto;
  }

  .map {
    flex: 0 0 40vh;
    height: 40vh;
  }

  .constituency-creator {
    flex: 0 0 40vh;
    height: 40vh;
  }

  .container {
    overflow: visible;
  }
}

@media (max-width: 600px) {
  .top-row {
    flex-direction: column;
    align-items: flex-start;
  }
  .controls {
    width: 100%;
    justify-content: flex-start;
  }
}
</style>