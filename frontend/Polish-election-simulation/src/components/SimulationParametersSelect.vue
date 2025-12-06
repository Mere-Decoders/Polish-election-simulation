<template>
  <form @submit.prevent="$emit('simulationParametersChosen', new SimulationParametersNotice(selectedVotesID, selectedMethodID, selectedConstituencySetID))">
    <label> Vote set: </label>
    <select v-model="selectedVotesID">
      <option v-for="votesID in votesIDs" :value="votesID">{{ votesID.name }}</option>
    </select>
    <label> Method: </label>
    <select v-model="selectedMethodID">
      <option v-for="methodID in methodIDs" :value="methodID">{{ methodID.name }}</option>
    </select>
    <label> Constituency set: </label>
    <select v-model="selectedConstituencySetID">
      <option v-for="constituencySetID in constituencySetIDs" :value="constituencySetID">{{ constituencySetID.name }}</option>
    </select>
    <input type="submit" value="płosze daj dane -w-" onclick>
  </form>
</template>

<script setup lang="ts">
  import ApportionmentMethodID from '../api/ApportionmentMethodID.ts'
  import VotesID from '../api/VotesID.ts'
  import ConstituencySetID from '../api/ConstituencySetID.ts'
  import SimulationParametersNotice from '../api/SimulationParametersNotice.ts'

  interface Props {
    methodIDs: ApportionmentMethodID[],
    votesIDs: VotesID[],
    constituencySetIDs: ConstituencySetID[],
  }

  const props = defineProps<Props>()
</script>
