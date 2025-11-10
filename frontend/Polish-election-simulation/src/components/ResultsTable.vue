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
    <tbody>
      <tr
        v-for='row in resultsToDisplay'
      >
        <td>
	  {{ row.partyName }}
	</td>
        <td>
	  {{ row.votes}}
	</td>
        <td>
	  {{ row.percentVotes}}
	</td>
        <td>
	  {{ row.seats}}
	</td>
        <td>
	  {{ row.percentSeats }}
	</td>
      </tr>
    </tbody>
  </table>
</template>

<script setup lang='ts'>
  import ResultsTableRow from '../api/ResultsTableRow.ts'

  const props = defineProps({
    resultsToDisplay: {
      type: Array,
      required: true,
    }
  })
</script>
