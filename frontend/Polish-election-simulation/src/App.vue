<template>
  <Menubar :model="items" class="nav">
    <template #item="{ item, props }">
      <router-link v-if="item.route" v-slot="{ href, navigate }" :to="item.route" custom>
        <a :href="href" v-bind="props.action" @click="navigate">
          <span>{{ item.label }}</span>
        </a>
      </router-link>
<!--      <a v-else :href="item.url" :target="item.target" v-bind="props.action">-->
<!--        <span>{{ item.label }}</span>-->
<!--      </a>-->
    </template>
  </Menubar>
  <main>
    <RouterView/>
  </main>
</template>

<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import { ref } from 'vue';
import Menubar from 'primevue/menubar'

const items = ref([
  { label: 'Home', route: '/' },
  { label: 'About', route: '/about' },
  { label: 'Election simulation', route: '/simulation' },
  { label: 'Login', route: '/login' }
]);
</script>

<style>

html {
  scrollbar-gutter: stable; /* Reserves space for scrollbar */
}

</style>

<style scoped>

body {
  font-size: 16px;
}

header {
  line-height: 1.5;
  max-height: 100vh;
}

main {
  width: 85%;
  max-width: 85%;
  text-align: left;
  justify-content: left;
  margin-left: auto;
  margin-right: auto;
}

.nav {
  display: flex;
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  justify-content: center;
  font-size: 1rem;
  text-align: center;
  background-color: var(--color-background); /* Add background so content doesn't show through */
  z-index: 100; /* Ensure nav stays on top */
}

main {
  padding-top: 80px; /* Add this to push content below the fixed nav */
}

nav a.router-link-exact-active {
  color: var(--color-text);
}

nav a.router-link-exact-active:hover {
  background-color: transparent;
}

nav a {
  color: var(--color-text-navigation);
  font-weight: bold;
  display: inline-block;
  padding: 0 1rem;
  border-left: 1px solid var(--color-border);
}

nav a:first-of-type {
  border: 0;
}

@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
    padding-right: calc(var(--section-gap) / 2);
  }

  .logo {
    margin: 0 2rem 0 0;
  }

  header .wrapper {
    display: flex;
    place-items: flex-start;
    flex-wrap: wrap;
  }

  nav {
    text-align: left;
    margin-left: -1rem;
    font-size: 1rem;

    padding: 1rem 0;
    margin-top: 1rem;
  }
}
</style>
