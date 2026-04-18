<template>
  <table>
    <thead>
      <tr>
        <th>Nazwa partii</th>
	<th class="numberCell">Liczba głosów</th>
	<th class="numberCell">Procent głosów</th>
	<th class="numberCell">Liczba mandatów</th>
	<th class="numberCell">Procent mandatów</th>
      </tr>
    </thead>
    <tbody class="mainTable">
      <tr
        v-for='row in processedResults'
      >
        <td>
	  {{ row.partyName }}
	</td>
        <td class="numberCell">
	  {{ row.votes}}
	</td>
        <td class="numberCell">
	  {{ row.percentVotes}}%
	</td>
        <td class="numberCell">
	  {{ row.seats}}
	</td>
        <td class="numberCell">
	  {{ row.percentSeats }}%
	</td>
      </tr>
    </tbody>
  </table>
</template>

<script setup lang='ts'>
  import ResultsTableRow from '../api/ResultsTableRow.ts'
  import { computed } from 'vue'

  const props = defineProps<{
    resultsToDisplay: ResultsTableRow[]
  }>()
  function formatPercentage(percentage: number) {
    return Math.floor(percentage * 10000) / 100;
  }

  const processedResults = computed(() => {
    let result = []
    for (let row of props.resultsToDisplay) {
      let newRow = {...row!}
      newRow.percentVotes = formatPercentage(newRow.percentVotes)
      newRow.percentSeats = formatPercentage(newRow.percentSeats)

      result.push(newRow);
    }
    return result
  })
</script>

<style scoped>
  @reference "tailwindcss";

  .numberCell {
    @apply text-center;
  }
  table {
    @apply w-[130%] overflow-hidden rounded-[30px] border-collapse;
  }
  td,
  th {
    @apply text-justify p-[5px];
  }
  table tr > :first-child,
  table th > :first-child {
    @apply pl-[50px];
  }
  table tr > :last-child,
  table th > :last-child {
    @apply pr-[30px];
  }
  tr:nth-child(odd) {
    @apply bg-[#222222];
  }
  tr:nth-child(even) {
    @apply bg-[#0a0a0a];
  }
  th {
    @apply bg-[darkred] text-[white] py-[15px];
  }
</style>
