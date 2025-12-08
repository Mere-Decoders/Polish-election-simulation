<template>
  <div class="about">
    <h1>This is an about page</h1>
  </div>
  <div>
    <Composition/>
  </div>

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
</style>
<script setup lang="ts">
import Composition from "@/components/Composition.vue";
</script>