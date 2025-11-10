<template>
  <ResultsTable :resultsToDisplay="resultsToDisplay"/>
</template>

<script setup lang='ts'>
  import { computed } from 'vue'

  import Results from '../../api/Results.ts'
  import ResultsTableRow from '../../api/ResultsTableRow.ts'
  import ResultsTable from '../ResultsTable.vue'

  const props = defineProps({
    results: Results
  })
  
  const resultsToDisplay = computed(() => {
    const result: ResultsTableRow[] = [
      new ResultsTableRow("Polska Partia Przyjaciół Piwa", 420, 13.3, 1, 50.0),
      new ResultsTableRow("Polska Partia Robotnicza", 422, 23.3, 2, 75.0),
    ]
    return result
  })
</script>
