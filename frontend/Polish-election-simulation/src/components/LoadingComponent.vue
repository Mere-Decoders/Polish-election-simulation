<template>
  <div class="w-full min-h-[220px] flex flex-col justify-center items-center gap-3" role="status" aria-live="polite" aria-label="Loading">
    <span class="w-10 h-10 border-[color:var(--color-border)] animate-spin rounded-full border-[0.3rem] border-solid" :style="{ borderTopColor: spinnerColor }" aria-hidden="true"></span>
    <p class="text-[color:var(--color-text)] text-[0.95rem]">{{ text }}</p>
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
