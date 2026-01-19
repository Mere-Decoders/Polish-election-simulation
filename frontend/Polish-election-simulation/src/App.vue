<template>
  <Menubar :model="items" class="nav">
    <template #item="{ item, props }">
      <router-link 
        v-if="item.route" 
        v-slot="{ href, navigate, isActive }" 
        :to="item.route"
      >
        <a 
          :href="href" 
          v-bind="props.action" 
          @click="navigate"
          :class="{ 'active-link': isActive }"
        >
          <span>{{ item.label }}</span>
        </a>
      </router-link>
      <a v-else :href="item.url" :target="item.target" v-bind="props.action">
        <span>{{ item.label }}</span>
      </a>
    </template>
  </Menubar>
  <main>
    <RouterView />
  </main>
</template>

<script setup lang="ts">
import { RouterView } from 'vue-router'
import { ref } from 'vue'
import Menubar from 'primevue/menubar'

const items = ref([
  { label: 'Strona główna', route: '/' },
  { label: 'O stronie', route: '/about' },
  { label: 'Symulacja wyborów', route: '/simulation' },
  { label: 'Logowanie', route: '/login' }
])
</script>

<style>
html {
  scrollbar-gutter: stable;
}

#app {
  max-width: 1500px;
  margin: 0 auto;
}
</style>

<style scoped>
main {
  width: 85%;
  max-width: 85%;
  text-align: left;
  margin: 1vw auto;
}

.nav {
  display: flex;
  justify-content: center;
  font-size: 0.9rem;
  text-align: center;
  padding: 0.5rem 0;
}

@media (min-width: 1024px) {
  .nav {
    text-align: left;
    font-size: 1rem;
  }
}
</style>
