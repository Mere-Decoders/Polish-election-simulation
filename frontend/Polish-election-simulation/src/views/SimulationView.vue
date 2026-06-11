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
        <div class="text-[1.2rem] font-semibold text-[rgba(0,0,0,0.72)] mt-0 mb-[0.6rem] mx-0 pt-0 pb-[0.35rem] px-0 border-b-[rgba(0,0,0,0.1)] border-b border-solid">
          <b>
            <span class="inline-block" v-if="currentConstituency">Constituency results</span>
            <span class="inline-block" v-else>National results</span>
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
import { generateConstituenciesFromDistrict } from "@/api/constituencyLoader.ts";

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
  const index = Number(mapStore.selectedConstituency) - 1;
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
const defaultGUID2 = "00000000-0000-0000-0000-000000000003";

onMounted(async () => {
  const defaultSimData = await apiClient.getSimulationData(defaultGUID2);
  constituencies.value = await generateConstituenciesFromDistrict(defaultSimData.districts);
  methods.value = await apiClient.getApportionmentMethodIDs();
  simData.value = await apiClient.getVotesIDs();
  detailedResults.value = await apiClient.getDetailedResults(defaultGUID2, defaultGUID);
  resultsToDisplay.value = await apiClient.getTotalResults(defaultGUID2, defaultGUID);
  isLoading.value = false;
});

function formatPercent(percent: number): string {
  return `${(percent * 100).toFixed(2)}%`;
}

async function loadNewResults() {
  isLoading.value = true;
  const [results, simDataResult] = await Promise.all([
      apiClient.getTotalResults(selectData.value.simData, selectData.value.method),
      apiClient.getSimulationData(selectData.value.simData),
  ]);
  resultsToDisplay.value = results;
  detailedResults.value = await apiClient.getDetailedResults(selectData.value.simData, selectData.value.method);
  constituencies.value = await generateConstituenciesFromDistrict(simDataResult.districts);
  resultsToDisplay.value = await apiClient.getTotalResults(selectData.value.simData, selectData.value.method);
  mapStore.selectConstituency(0);
  isLoading.value = false;
}

</script>