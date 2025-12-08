<template>
  <div class="container">
    <div class="header">
      <h1>Testing, attention please</h1>
    </div>
    <div class="map">
      <h2>This is supposed to be a map</h2>
    </div>
    <div class="seats">
      just seats
    </div>
  </div>
<!--  <div class="about">-->
<!--    <h1>This is an about page</h1>-->
<!--  </div>-->
<!--  <div>-->
<!--    <Composition/>-->
<!--  </div>-->

</template>

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
        / 1fr 1fr;
  margin-left: auto;
  margin-right: auto;
}

.header {
  grid-area: header;
}

.map {
  grid-area: map;
}

.seats {
  grid-area: seats;
}
</style>
<script setup lang="ts">
import Composition from "@/components/Composition.vue";
</script>