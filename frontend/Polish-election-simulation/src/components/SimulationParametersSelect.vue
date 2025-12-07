<template>
  <form class="simulation_parameters_select" @submit.prevent="$emit('simulationParametersChosen', new SimulationParametersNotice(selectedVotesID, selectedMethodID, selectedConstituencySetID))">
    <div class="selects">
      <label> Zestaw głosów: </label>
      <select v-model="selectedVotesID">
        <option v-for="votesID in votesIDs" :value="votesID">{{ votesID.name }}</option>
      </select>
      <label> Metoda zliczania: </label>
      <select v-model="selectedMethodID">
        <option v-for="methodID in methodIDs" :value="methodID">{{ methodID.name }}</option>
      </select>
      <label> Podział na okręgi: </label>
      <select v-model="selectedConstituencySetID">
        <option v-for="constituencySetID in constituencySetIDs" :value="constituencySetID">{{ constituencySetID.name }}</option>
      </select>
    </div>
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
<style>
  .simulation_parameters_select {
    width: max-content;
    padding: 20px;
    padding-bottom: 5px;
    border: 2px solid red;
    border-radius: 8px;
  }

  .selects {
    display: grid;
    grid-template-columns: max-content max-content;
    place-items: center;
    gap: 20px;
  }
  
  .simulation_parameters_select select {
    background-color: #222222;
    color: white;
    border: 2px solid darkred;
    border-radius: 8px;
    padding: 10px 15px;
    font-size: 14px;
    width: 100%;
    max-width: 300px;
    cursor: pointer;
    transition: all 0.3s ease;
  }
  
  .simulation_parameters_select select:hover {
    background-color: #2a2a2a;
    border-color: #c41e3a;
  }
  
  .simulation_parameters_select select:focus {
    outline: none;
    background-color: #0a0a0a;
    border-color: darkred;
    box-shadow: 0 0 5px rgba(139, 0, 0, 0.5);
  }
  
  .simulation_parameters_select select option {
    background-color: #222222;
    color: white;
    padding: 10px;
  }
  
  .simulation_parameters_select select option:hover {
    background-color: darkred;
  }
  
  .simulation_parameters_select label {
    color: white;
    font-weight: bold;
    margin-right: 10px;
    min-width: 120px;
  }

  .simulation_parameters_select input {
    background-color: darkred;
    color: white;
    border: none;
    border-radius: 8px;
    padding: 12px 30px;
    font-size: 14px;
    font-weight: bold;
    cursor: pointer;
    transition: all 0.3s ease;

    display: block;
    margin: 15px auto;
  }
  
  .simulation_parameters_select input:hover {
    background-color: #c41e3a;
    box-shadow: 0 0 10px rgba(139, 0, 0, 0.6);
    transform: translateY(-2px);
  }
  
  .simulation_parameters_select input:active {
    transform: translateY(0);
    box-shadow: 0 0 5px rgba(139, 0, 0, 0.4);
    background-color: white;
    transition: all 0.05s ease;
    color: #8b0000;
  }
  
  .simulation_parameters_select input:focus {
    outline: none;
    box-shadow: 0 0 8px rgba(139, 0, 0, 0.8);
  }
</style>
