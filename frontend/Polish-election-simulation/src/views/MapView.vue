<script setup lang="ts">
    import { onMounted } from "vue";
    import { generateConstituencies } from "@/api/constituencyLoader";
    console.log("lol");
    let content = await generateConstituencies();
    
    console.log(content);
</script>

<template>
  <div class="about">
    <h1>testing maps</h1>
  </div>
  <div class="map">
    not testing maps?
  </div>
</template>

<style>
@media (min-width: 1024px) {
  .about {
    min-height: 100vh;
    display: flex;
    align-items: center;
  }
}
</style>