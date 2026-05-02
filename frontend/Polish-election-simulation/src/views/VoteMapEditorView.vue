<template>
  <LoadingComponent v-if="isLoading" spinner-color="lightgreen" />
  <div v-else>
    <div class="container">
      <div class="top-row">
        <div class="title-section">
          <h1>Votes & map editor</h1>
        </div>
        <div class="controls">
          <Button label="Upload" @click=""/>
        </div>
      </div>
      <div class="map-palette-row">
        <div class="map">
          <ConstituencyEditorMap
              :constituencies="powiats"
              :fillMap="fillMap"
              @feature-click="handleFeatureClick"
          />
        </div>
        <div class="constituency-creator">
          <div class="palette-toolbar">
            <button
                :class="{ active: eraserActive }"
                @click="toggleEraser"
                title="Eraser"
            >Erase</button>
            <button
                @click="addGroupToView"
                :disabled="visibleGroupIds.size >= 100"
                title="Add group"
            >+</button>
          </div>
          <div class="group-list">
            <div
                v-for="group in visibleGroups"
                :key="group.id"
                class="group-pill"
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
          <div v-for="column in columns" :key="column.id" class="column-pill">
            <input
                v-model="column.name"
                :class="{ error: column.name.trim() === '' }"
                @blur="validateName(column)"
                @keydown.enter="($event.target as HTMLInputElement).blur()"
                @keydown.esc="revert(column, $event)"
            />
            <button @click="removeColumn(column.id)">×</button>
          </div>
          <button class="add-btn" @click="addColumn">+</button>
        </div>
      </div>
      <table>
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
</template>

<script setup lang="ts">
import {computed, nextTick, onMounted, ref} from "vue";
import LoadingComponent from "@/components/LoadingComponent.vue";
import {loadPowiaty} from "@/api/constituencyLoader.ts";
import Button from "primevue/button";
import {loadColors} from "@/api/colorLoader.ts";
import ConstituencyEditorMap from "@/components/ConstituencyEditorMap.vue";

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
  powiats: Set<string>
}
const rowGroups = ref<RowGroup[]>([]);

const selectedGroupId = ref<number | null>(null);
const eraserActive = ref(false);
const visibleGroupIds = ref<Set<number>>(new Set());
const visibleGroups = computed(() =>
    rowGroups.value.filter(g => visibleGroupIds.value.has(g.id))
);

onMounted(async () => {
  powiats.value = await loadPowiaty();
  for (const powiat of powiats.value.features) {
    rows.value.push(
        { terc: powiat.properties.terc, name: powiat.properties.name, party1: 0, party2: 0 }
    );
  }
  const colors = await loadColors();
  let i = 1;
  for (const color of colors) {
    rowGroups.value.push(
        { id: i++, color, powiats: new Set() }
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

</script>

<style>
</style>