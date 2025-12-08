<template>
  <div class="container">
    <div class="header">
      <h1>Simulation</h1>
    </div>
    <div class="map">
      <PowiatMap/>
    </div>
    <div class="seats">
      <VisualizationTab/>
    </div>
  </div>
<!--  <div>-->
<!--    <Composition/>-->
<!--  </div>-->

</template>

<script setup lang="ts">
import Composition from "@/components/Composition.vue";
import PowiatMap from "@/components/PowiatMap.vue";
import VisualizationTab from "@/components/tabs/VisualizationTab.vue";
</script>

<style>
@media (min-width: 1024px) {
  .about {
    min-height: 100vh;
    display: flex;
    align-items: center;
    width: 100%;
  }
}

.container {
  display: grid;
  grid: "header header" 50px
        "map seats" 1fr
        / 50% 50%;
  margin-left: auto;
  margin-right: auto;
  height: 100vh;
  width: 100%;
}

.header {
  grid-area: header;
  justify-self: center;
}

.map {
  grid-area: map;
  min-width: 0;
  min-height: 0;
  width: 100%;
  height: 100%;
}

.seats {
  grid-area: seats;
}
</style>