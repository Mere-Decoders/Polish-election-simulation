<template>
  <table class="w-[130%] overflow-hidden rounded-[30px] border-collapse">

    <thead>
      <tr class="[&>th]:text-justify [&>th]:p-[5px] [&>th]:py-[15px] [&>th]:bg-[darkred] [&>th]:text-white [&>th]:first:pl-[50px] [&>th]:last:pr-[30px]">
        <th>Nazwa partii</th>
        <th class="text-center">Liczba głosów</th>
        <th class="text-center">Procent głosów</th>
        <th class="text-center">Liczba mandatów</th>
        <th class="text-center">Procent mandatów</th>
      </tr>
    </thead>

    <tbody class="[&>tr:nth-child(odd)]:bg-[#222222] [&>tr:nth-child(even)]:bg-[#0a0a0a] [&>td]:p-[5px] [&>td]:first:pl-[50px] [&>td]:last:pr-[30px]">
      <tr v-for="row in processedResults" :key="row.partyName">

        <td class="text-justify">{{ row.partyName }}</td>
        <td class="text-center">{{ row.votes }}</td>
        <td class="text-center">{{ row.percentVotes }}%</td>
        <td class="text-center">{{ row.seats }}</td>
        <td class="text-center">{{ row.percentSeats }}%</td>

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


</style>
