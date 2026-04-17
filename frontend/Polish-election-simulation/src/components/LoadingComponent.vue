<template>
  <div class="loading-wrapper" role="status" aria-live="polite" aria-label="Loading">
    <span class="spinner" :style="{ borderTopColor: spinnerColor }" aria-hidden="true"></span>
    <p class="loading-text">{{ text }}</p>
  </div>
</template>

<script setup lang="ts">
withDefaults(defineProps<{
  text?: string;
  spinnerColor?: string;
}>(), {
  text: "Loading...",
  spinnerColor: "#e14b4b"
});
</script>

<style scoped>
  @reference "tailwindcss";

  .loading-wrapper {
    @apply w-full min-h-[220px] flex flex-col justify-center items-center gap-3;
  }
  .spinner {
    @apply w-10 h-10 border-[color:var(--color-border)] animate-spin rounded-full border-[0.3rem] border-solid;
  }
  .loading-text {
    @apply text-[color:var(--color-text)] text-[0.95rem];
  }
</style>
