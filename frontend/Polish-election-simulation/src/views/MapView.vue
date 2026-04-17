<template>
  <div class="about">
    <h1>testing maps</h1>
  </div>
  <div class="map">
    not testing maps?
  </div>
</template>

<script setup lang="ts">
</script>

<style>
  @reference "tailwindcss";
  
  @media (min-width: 1024px) {
    .about {
      @apply min-h-screen flex items-center;
    }
  }
</style>