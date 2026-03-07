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
  spinnerColor: "var(--tomato-jam)"
});
</script>

<style scoped>
.loading-wrapper {
  width: 100%;
  min-height: 220px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  gap: 0.75rem;
}

.spinner {
  width: 2.5rem;
  height: 2.5rem;
  border: 0.3rem solid var(--color-border);
  border-top-color: var(--tomato-jam);
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

.loading-text {
  color: var(--color-text);
  font-size: 0.95rem;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}
</style>
