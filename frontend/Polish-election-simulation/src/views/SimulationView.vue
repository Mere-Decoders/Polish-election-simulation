<template>
  <div v-if="isLoading">
    <p>Loading...</p>
  </div>
  <div v-else>
    <div class="container">
      <div class="header">
        <h1>Symulacja</h1>
      </div>
      <div class="map">
        <PowiatMap :constituencies="constituencies"/>
      </div>
      <div class="select">
        <Select
          v-model="selectData.simData"
          :options="simData"
          optionLabel="name"
          optionValue="id"
          placeholder="Select data"
        />
        <Select
            v-model="selectData.method"
            :options="methods"
            optionLabel="name"
            optionValue="id"
            placeholder="Select method"
        />
        <Button label="View" @click="loadNewResults"/>
      </div>
      <div class="seats">
        <Parliament
            :resultsToDisplay="resultsToDisplay"
            :innerRadius = "50"
            :outerRadius = "190"
        />
      </div>
      <div class="table">
        <DataTable :value="resultsToDisplay">
	  <Column header="">
            <template #body="slotProps">
	      <svg :width="10" :height="10" xmlns="http://www.w3.org/2000/svg">
                <circle
                  :cx="5"
                  :cy="5"
                  :r="5"
                  :fill="slotProps.data.color"
		/>
              </svg>
	    </template>
	  </Column>
          <Column field="partyName" header="Nazwa partii"></Column>
          <Column field="votes" header="Liczba głosów"></Column>
          <Column field="percentVotes" header="Procent głosów">
            <template #body="slotProps">
              {{ formatPercent(slotProps.data.percentVotes) }}
            </template>
          </Column>
          <Column field="seats" header="Liczba mandatów"></Column>
          <Column field="percentSeats" header="Procent mandatów">
            <template #body="slotProps">
              {{ formatPercent(slotProps.data.percentSeats) }}
            </template>
          </Column>
        </DataTable>
      </div>
    </div>
  </div>

</template>

<script setup lang="ts">
import { onMounted, ref } from "vue";

import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Select from 'primevue/select';
import Button from 'primevue/button';

import PowiatMap from "@/components/PowiatMap.vue";
import Parliament from "@/components/Parliament.vue";

import apiClient from "@/api/apiClient.ts";
import ResultsTableRow from "@/api/ResultsTableRow.ts";
import {generateConstituencies} from "@/api/constituencyLoader.ts";
import ApportionmentMethod from "@/api/ApportionmentMethod.ts";
import VotesID from "@/api/VotesID.ts";
const _apiClient = apiClient.getInstance();

const resultsToDisplay = ref<ResultsTableRow[]>([]);

// TODO: this job should probably be relegated to PowiatMap and emit upon successful onMounted
// to guarantee the map is fully rendered by the time we pull down the loading screen
const constituencies = ref<any>(null);
const isLoading = ref(true);

const methods = ref<ApportionmentMethod[]>([]);
const simData = ref<VotesID[]>([]);
const selectData = ref({
  method: "",
  simData: ""
})

onMounted(async () => {
  constituencies.value = await generateConstituencies();
  methods.value = await apiClient.getApportionmentMethodIDs();
  simData.value = await apiClient.getVotesIDs();
  resultsToDisplay.value = await apiClient.getTotalResults("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000");
  isLoading.value = false;
});

function formatPercent(percent: number): string {
  return `${(percent * 100).toFixed(2)}%`;
}

async function loadNewResults() {
  isLoading.value = true;
  resultsToDisplay.value = await apiClient.getTotalResults(selectData.value.simData, selectData.value.method);
  isLoading.value = false;
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
  display: grid;
  grid: "header header" 50px
        "select select" 50px
        "map seats" 40vh
        "table table" auto
        / 50% 50%;
  margin-left: auto;
  margin-right: auto;
  height: 100vh;
  width: 100%;
}

.header {
  grid-area: header;
  justify-self: center;
}

.map {
  grid-area: map;
  min-width: 0;
  min-height: 0;
  width: 100%;
  height: 100%;
}

.seats {
  grid-area: seats;
}

.table {
  grid-area: table;
}

.select {
  grid-area: select;
}
</style>