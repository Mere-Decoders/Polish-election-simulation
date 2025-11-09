<template>
  <div class="tabs">
    <button
      v-for="(tab, index) in tabs"
      :key="index"
      @click="activeTab = tab.name"
      :class="{ active: activeTab === tab.name}"
    >
      {{ tab.title }}
    </button>
  </div>

  <div class="tab-content"
      v-for="(tab, index) in tabs"
      v-show="activeTab === tab.name"
  >
    <slot :name="tab.name"></slot>
  </div>
</template>

<script setup>
import { ref, toRefs } from 'vue'

const props = defineProps({
  tabs: {
    type: Array,
    required: true,
    // [{ name: 'home', title: 'Home' }, { name: 'about', title: 'About' }]
  },
})

const activeTab = ref(0)
activeTab.value = props.tabs[0].name
</script>

<style scoped>
.tabs {
  display: flex;
  gap: 0.5rem;
}
.active {
  font-weight: bold;
  text-decoration: underline;
}
.tab-content {
  margin-top: 1rem;
}
</style>

