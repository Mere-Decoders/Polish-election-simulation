<template>
  <Parliament 
    :resultsToDisplay="resultsToDisplay"
    :innerRadius = "100"
    :outerRadius = "300"
  />
  <ResultsTable :resultsToDisplay="resultsToDisplay"/>
  <SimulationParametersSelect
    :methodIDs="apiClient.getApportionmentMethodIDs()"
    :votesIDs="apiClient.getVotesIDs()"
    :constituencySetIDs="apiClient.getConstituencySetIDs()"
    @simulationParametersChosen="handleSimulationParametersChosen"/>
</template>

<script setup lang='ts'>
import { computed, onMounted, ref } from 'vue';

  import apiClient from "@/api/apiClient.ts";
  import Results from '@/api/Results.ts';
  import ResultsTableRow from '@/api/ResultsTableRow.ts';
  import ResultsTable from '../ResultsTable.vue';
  import Parliament from '../Parliament.vue';
  import SimulationParametersSelect from '../SimulationParametersSelect.vue'
  import SimulationParametersNotice from '@/api/SimulationParametersNotice.ts'

  const _apiClient = apiClient.getInstance();
  const props = defineProps({
    results: Results
  });
  
  // const resultsToDisplay = computed(() => {
  //   const result: ResultsTableRow[] = [
  //     new ResultsTableRow("Polska Partia Przyjaciół Piwa", 420, 13.3, 1, 50.0),
  //     new ResultsTableRow("Polska Partia Robotnicza", 422, 23.3, 2, 75.0),
  //   ]
  //   return result;
  // });

  let resultsToDisplay = ref<ResultsTableRow[]>([]);

  onMounted(async () => {
    resultsToDisplay.value = await _apiClient.getTotalResults(2023, "D'Hondt");
  });

  function handleSimulationParametersChosen(parameters: SimulationParametersNotice) {
    console.log('Parámetros recibidos:', parameters);
    async() => {resultsToDisplay.value = await _apiClient.getTotalResults(parameters.votesID.name, parameters.methodID)};
  }

</script>
