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
      <div class="table">
        <div class="table-header">
          <b>
            <span v-if="currentConstituency">Constituency results</span>
            <span v-else>National results</span>
          </b>
        </div>
        <span v-if="currentConstituency">Press selected constituency again to see national results.</span>
        <span v-else>Press a constituency on the map to see its results.</span>
        <DataTable :value="selectedConstituencyResults">
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
import {computed, type ComputedRef, onMounted, ref} from "vue";

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

import { useMapStore } from '@/stores/useMapStore.ts'
import { storeToRefs } from 'pinia'
import DetailedResultsRow from "@/api/DetailedResultsRow.ts";
import get_color_for_index from "@/api/get_color_for_index.ts";

const mapStore = useMapStore()
const { currentConstituency } = storeToRefs(mapStore)

const _apiClient = apiClient.getInstance();

const detailedResults = ref<DetailedResultsRow[]>([]);
const totalResults: ComputedRef<ResultsTableRow[]> = computed(() => {
  const partyCount = detailedResults.value.length;
  let results: ResultsTableRow[] = [];
  let sumSeatsArray: number[] = [];
  let sumVotesArray: number[] = [];
  for (let i = 0; i < partyCount; i++) {
    let sumVotes: number = 0;
    let sumSeats: number = 0;
    for (let j = 0; j < detailedResults.value[i]!.votes.length; j++) {
      sumVotes += detailedResults.value[i]!.votes[j]!;
      sumSeats += detailedResults.value[i]!.seats[j]!;
    }
    sumSeatsArray.push(sumSeats);
    sumVotesArray.push(sumVotes);
  }
  const totalSumVotes: number = sumVotesArray.reduce((a, b) => a + b);
  const totalSumSeats: number = sumSeatsArray.reduce((a, b) => a + b);
  for (let i = 0; i < partyCount; i++) {
    results.push(new ResultsTableRow(
        detailedResults.value[i]!.partyName,
        get_color_for_index(i, partyCount),
        sumVotesArray[i]!,
        sumVotesArray[i]! / totalSumVotes,
        sumSeatsArray[i]!,
        sumSeatsArray[i]! / totalSumSeats
    ));
  }
  return results;
});

const selectedConstituencyResults: ComputedRef<ResultsTableRow[]> = computed(() => {
  if (mapStore.selectedConstituency === 0) {
    return totalResults.value;
  }
  const index = Number(mapStore.selectedConstituency);
  let results: ResultsTableRow[] = [];
  const totalSumVotes: number = detailedResults.value.reduce(
      (a, b) => a + b.votes[index]!, 0
  )
  const totalSumSeats: number = detailedResults.value.reduce(
      (a, b) => a + b.seats[index]!, 0
  )
  for (let i = 0; i < detailedResults.value.length; i++) {
    const votes = detailedResults.value[i]!.votes[index]!;
    const seats = detailedResults.value[i]!.seats[index]!;
    results.push(new ResultsTableRow(
        detailedResults.value[i]!.partyName,
        get_color_for_index(i, detailedResults.value.length),
        votes,
        votes / totalSumVotes,
        seats,
        seats / totalSumSeats
    ));
  }
  return results;
});

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

const defaultGUID = "00000000-0000-0000-0000-000000000000";

onMounted(async () => {
  constituencies.value = await generateConstituencies();
  methods.value = await apiClient.getApportionmentMethodIDs();
  simData.value = await apiClient.getVotesIDs();
  detailedResults.value = await apiClient.getDetailedResults(defaultGUID, defaultGUID);
  resultsToDisplay.value = await apiClient.getTotalResults(defaultGUID, defaultGUID);
  isLoading.value = false;
});

function formatPercent(percent: number): string {
  return `${(percent * 100).toFixed(2)}%`;
}

async function loadNewResults() {
  isLoading.value = true;
  mapStore.selectConstituency(0);
  detailedResults.value = await apiClient.getDetailedResults(selectData.value.simData, selectData.value.method);
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

.table-header {
  margin: 0 0 0.6rem;
  padding: 0 0 0.35rem;
  border-bottom: 1px solid rgba(0, 0, 0, 0.1);
  font-size: 1.2rem;
  font-weight: 600;
  color: rgba(0, 0, 0, 0.72);
}

.table-header span {
  display: inline-block;
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
