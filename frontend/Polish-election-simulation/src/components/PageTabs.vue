<template>
  <div class="tabs">
    <button
      v-for="(tab, index) in tabs"
      :key="index"
      @click="activeTab = tab.name"
      :class="{ active: activeTab === tab.name}"
    >
      >&nbsp{{ tab.title }}
    </button>
  </div>

  <div class="tab-content"
      v-for="(tab, index) in tabs"
      v-show="activeTab === tab.name"
  >
    <slot :name="tab.name"></slot>
  </div>
</template>

<script setup lang="ts">
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
  flex-direction: column;
  width: max-content;
  height: max-content;
  gap: 0.5rem;

  border-left: 3px solid darkred;
  padding-left: 10px;
}
.active {
  font-weight: bold;
  text-decoration: underline;
}
.tab-content {
  margin-top: 1rem;
}

.tabs button {
  text-align: left;
  padding: 5px;
  border: 2px solid white;
  background-color: transparent;
  border-radius: 10px;
  color: white;
  transition: all 0.3s ease;
  font-size: large;
}

.tabs button:hover {
  background-color: darkred;
  border-color: darkred;
  box-shadow: 0 0 100px rgba(139, 0, 0, 1.0);
  transform: translateX(10px);
  cursor: pointer;
}

.tabs button:active {
  transform: translateX(0px);
  box-shadow: 0 0 10px rgba(255, 255, 255, 0.6);
  background-color: white;
  color: darkred;
  border-color: white;
  transition: all 0.05s ease;
}

.tabs .active {
  color: white;
  background-color: darkred;
  border-color: darkred;
  text-decoration: none;
}

</style>
