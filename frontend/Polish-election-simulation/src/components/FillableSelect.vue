<template>
    <select>
        <option v-for="el in elements">{{ el.name }}</option>
    </select>
</template>

<script setup>
const props = defineProps({
  elements: {
    type: Array,
    default: () => []
  }
})
</script>
