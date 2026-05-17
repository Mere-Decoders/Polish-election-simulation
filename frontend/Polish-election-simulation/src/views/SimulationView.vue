<template>
  <LoadingComponent v-if="isLoading" spinner-color="lightgreen" />
  <div v-else>
    <div class="flex flex-col w-full mx-auto my-0 max-md:overflow-visible">
      <div class="shrink-0 flex justify-between gap-2.5 p-8 max-[600px]:flex-col max-[600px]:items-start">
        <div class="flex-1 text-center text-3xl">
          <h1>Simulation</h1>
        </div>
        
        <div class="flex flex-wrap gap-2.5 items-center max-[600px]:w-full max-[600px]:justify-start">
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
      <div class="flex flex-wrap min-h-0 min-[769px]:h-[50vh] max-md:flex-col max-md:flex-[0_0_auto] max-md:min-h-0 max-md:h-auto">
        <div class="flex-[1_0_300px] h-full overflow-hidden max-md:flex-[0_0_40vh] max-md:h-[40vh]">
          <PowiatMap :constituencies="constituencies"/>
        </div>
        <div class="flex-[1_0_300px] h-full flex justify-center items-center overflow-hidden max-md:flex-[0_0_40vh] max-md:h-[40vh]">
          <Parliament
              :resultsToDisplay="resultsToDisplay"
              :innerRadius = "50"
              :outerRadius = "190"
          />
        </div>
      </div>
      <div class="flex-[0_0_auto] overflow-auto p-2.5">
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
