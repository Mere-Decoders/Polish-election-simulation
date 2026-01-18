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

<style>
  .numberCell {
    text-align: center;
  }

  table {
    border-collapse: collapse;
    width: 130%;
    border-radius: 30px;
    overflow: hidden;
  }

  td, th {
    padding: 5px;
    text-align: justify;
  }

  table tr > :first-child,
  table th > :first-child {
    padding-left: 50px;
  }
  
  table tr > :last-child,
  table th > :last-child {
    padding-right: 30px;
  }

  tr:nth-child(odd) {
    background-color: #222222;
  }

  tr:nth-child(even) {
    background-color: #0a0a0a;
  }

  th {
    padding-top: 15px;
    padding-bottom: 15px;
    background-color: darkred;
    color: white;
  }
</style>
