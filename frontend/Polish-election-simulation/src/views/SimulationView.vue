<template>
  <LoadingComponent v-if="isLoading" spinner-color="lightgreen" />
  <div v-else>
    <div class="container">
      <div class="top-row">
        <div class="title-section">
          <h3>Simulation</h3>
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
import LoadingComponent from "@/components/LoadingComponent.vue";

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
@reference "tailwindcss";

@media (min-width: 1024px) {
  .about {
    @apply min-h-screen flex items-center w-full;
  }
}
.container {
  @apply flex flex-col w-full mx-auto my-0;

  /* no fixed height, let content determine it */
}
.top-row {
  @apply shrink-0 flex justify-between gap-2.5 p-8;
}
.title-section {
  @apply flex-1 text-center text-3xl;
}
.controls {
  @apply flex flex-wrap gap-2.5 items-center;
}
.map-seats-row {
  @apply flex flex-wrap min-h-0;
}

/* ensure a sensible row height when items are side-by-side */
@media (min-width: 769px) {
  .map-seats-row {
    @apply h-[50vh];
  }
}
.map {
  @apply flex-[1_0_300px] h-full overflow-hidden;
}
.seats {
  @apply flex-[1_0_300px] h-full flex justify-center items-center overflow-hidden;
}
.table {
  @apply flex-[0_0_auto] overflow-auto p-2.5;
}
@media (max-width: 768px) {
  .map-seats-row {
    @apply flex-col flex-[0_0_auto] min-h-0 h-auto;

    /* size to children */
  }
  .map {
    @apply flex-[0_0_40vh] h-[40vh];
  }
  .seats {
    @apply flex-[0_0_40vh] h-[40vh];
  }
  .container {
    @apply overflow-visible;
  }
}
@media (max-width: 600px) {
  .top-row {
    @apply flex-col items-start;
  }
  .controls {
    @apply w-full justify-start;
  }
}

</style>
