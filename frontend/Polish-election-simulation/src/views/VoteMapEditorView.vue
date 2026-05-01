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
      <div class="table">
        <DataTable :value="rows">
          <Column field="name" header="Powiat name"></Column>
          <Column
              v-for="column in columns"
              :key="column.id"
              :field="'party' + String(column.id)"
              :header="column.name"
          />
        </DataTable>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import {nextTick, onMounted, ref} from "vue";
import LoadingComponent from "@/components/LoadingComponent.vue";
import {loadPowiaty} from "@/api/constituencyLoader.ts";
import Column from "primevue/column";
import PowiatMap from "@/components/PowiatMap.vue";
import Button from "primevue/button";
import DataTable from "primevue/datatable";
import VoteMapEditorColumn from "@/types/VoteMapEditorColumn.ts";

const isLoading = ref(true);
const powiats = ref<any>(null);
const columns = ref<VoteMapEditorColumn[]>([
  { id: 1, name: "Party 1", prev: "Party 1"},
  { id: 2, name: "Party 2", prev: "Party 2"}
]);
const rows = ref<any>([]);

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
  columns.value = columns.value.filter(c => c.id !== id)
}

let idCounter = columns.value.length + 1;

async function addColumn() {
  const id = idCounter++;
  const name = `Party ${columns.value.length + 1}`;
  columns.value.push({ id, name, prev: name });

  await nextTick();
  const inputs = document.querySelectorAll<HTMLInputElement>('.column-pill input')
  inputs[inputs.length - 1]?.select();
}

</script>

<style>
@media (min-width: 1024px) {
  .about {
    min-height: 100vh;
    display: flex;
    align-items: center;
  }
}
</style>