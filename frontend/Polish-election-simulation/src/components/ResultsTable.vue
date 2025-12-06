<template>
  <table>
    <thead>
      <tr>
        <th>Nazwa partii</th>
	<th>Liczba głosów</th>
	<th>Procent głosów</th>
	<th>Liczba mandatów</th>
	<th>Procent mandatów</th>
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
  import {computed} from 'vue'

  const props = defineProps({
    resultsToDisplay: {
      type: Array,
      required: true,
    }
  })

  function formatPercentage(percentage: number) {
    return Math.floor(percentage * 10000) / 100;
  }

  const processedResults = computed(() => {
    let result = []
    for (let row of props.resultsToDisplay) {
      let newRow = {...row}
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
    width: 100%;
  }

  td, tr {
    padding: 5px;
  }

  tr:nth-child(odd) {
    background-color: #ff0000;
  }

  tr:nth-child(even) {
    background-color: #bbbbbb;
  }

</style>
