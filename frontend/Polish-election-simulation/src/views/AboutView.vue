<template>
  <h1>This is the about page</h1>
  <a href="https://www.flaticon.com/free-icons/ballot" title="ballot icons">Ballot icons created by juicy_fish - Flaticon</a>
  <textarea v-model="code" placeholder="Napisz mi coś w Lule"/>
  <button @click="run">Uruchom</button>
  <pre v-if="result">{{ result }}</pre>

</template>

<script setup lang="ts">
import { ref } from 'vue';
import apiClient from "@/api/apiClient.ts";

const code = ref('');
const result = ref(null);
const error = ref(null);

async function run() {
  result.value = null;
  error.value = null;
  const answer = await apiClient.runLua(code.value);
  result.value = answer;
}

</script>

<style>
</style>
