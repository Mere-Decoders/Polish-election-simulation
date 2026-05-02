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
          <PowiatMap :constituencies="powiats"/>
        </div>
        <div class="palette">
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
        <tr v-for="row in rows" :key="row.name">
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
import {nextTick, onMounted, ref} from "vue";
import LoadingComponent from "@/components/LoadingComponent.vue";
import {loadPowiaty} from "@/api/constituencyLoader.ts";
import PowiatMap from "@/components/PowiatMap.vue";
import Button from "primevue/button";
import VoteMapEditorColumn from "@/types/VoteMapEditorColumn.ts";

const isLoading = ref(true);
const powiats = ref<any>(null);

const columns = ref<VoteMapEditorColumn[]>([
  { id: 1, name: "Party 1", prev: "Party 1"},
  { id: 2, name: "Party 2", prev: "Party 2"}
]);
let idCounter = columns.value.length + 1;


interface VoteMapEditorRow {
  name: string,
  [key: string]: any
}
const rows = ref<VoteMapEditorRow[]>([]);

onMounted(async () => {
  powiats.value = await loadPowiaty();
  console.log(powiats.value.features);
  for (const powiat of powiats.value.features) {
    rows.value.push(
        { name: powiat.properties.name, party1: 0, party2: 0 }
    );
  }
  isLoading.value = false;
});

function validateName(column: VoteMapEditorColumn) {
  if (column.name.trim() === '') {
    column.name = column.prev;
  }
  else {
    column.prev = column.name;
  }
}

function revert(column: VoteMapEditorColumn, event: KeyboardEvent) {
  column.name = column.prev;
  (event.target as HTMLInputElement).blur()
}



function removeColumn(id: number) {
  columns.value = columns.value.filter(c => c.id !== id);
  const field = `party${id}`;
  rows.value = rows.value.map(row => {
    const { [field]: _, ...rest } = row
    return rest
  }) as VoteMapEditorRow[];
}



async function addColumn() {
  const id = idCounter++;
  const name = `Party ${columns.value.length + 1}`;
  columns.value.push({ id, name, prev: name });
  const field = `party${id}`
  rows.value = rows.value.map(row => ({ ...row, [field]: 0 }))

  await nextTick();
  const inputs = document.querySelectorAll<HTMLInputElement>('.column-pill input')
  inputs[inputs.length - 1]?.select();
}

</script>

<style>
</style>