<template>
  <div v-if="isLoading">
    <p>Loading...</p>
  </div>
  <div v-else>
    <div class="container">
      <div class="top-row">
        <div class="title-section">
          <h1>Simulation</h1>
        </div>
        
        <div class="controls">
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
      </div>
      <div class="map-seats-row">
        <div class="map">
          <PowiatMap :constituencies="constituencies"/>
        </div>
        <div class="seats">
          <Parliament
              :resultsToDisplay="resultsToDisplay"
              :innerRadius = "50"
              :outerRadius = "190"
          />
        </div>
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

.map-seats-row {
  display: flex;
  flex-wrap: wrap;
  min-height: 0;
}

/* ensure a sensible row height when items are side-by-side */
@media (min-width: 769px) {
  .map-seats-row {
    height: 50vh;
  }
}

.map {
  flex: 1 0 300px;
  height: 100%;
  overflow: hidden;
}

.seats {
  flex: 1 0 300px;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  overflow: hidden;
}

.table {
  flex: 0 0 auto;
  overflow: auto;
  padding: 10px;
}

@media (max-width: 768px) {
  .map-seats-row {
    flex-direction: column;
    flex: 0 0 auto; /* size to children */
    min-height: 0;
    height: auto;
  }
  
  .map {
    flex: 0 0 40vh;
    height: 40vh;
  }
  
  .seats {
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