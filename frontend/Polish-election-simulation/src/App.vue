<template>
  <Menubar :model="items" class="nav">
    <template #item="{ item, props }">
      <router-link v-if="item.route" v-slot="{ href, navigate }" :to="item.route" custom>
        <a :href="href" v-bind="props.action" @click="navigate" :class="{ 'active-nav-item': route.path === item.route }">
          <span>{{ item.label }}</span>
        </a>
      </router-link>
      <a v-else :href="item.url" :target="item.target" v-bind="props.action">
        <span>{{ item.label }}</span>
      </a>
    </template>
  </Menubar>
  <main>
    <RouterView/>
  </main>
</template>

<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import { computed } from 'vue';
import Menubar from 'primevue/menubar'
import { useAuth } from '@/auth/useAuth';
import { useRoute } from 'vue-router';

const route = useRoute();

type NavItem = {
  label: string;
  route?: string;
  url?: string;
  target?: string;
};

const auth = useAuth();

const items = computed<NavItem[]>(() => [
  { label: 'Home', route: '/' },
  { label: 'About', route: '/about' },
  { label: 'Election simulation', route: '/simulation' },
  auth.isAuthenticated.value
    ? { label: 'Logout', route: '/logout' }
    : { label: 'Login', route: '/login' }
]);
</script>

<style scoped>
  @reference "tailwindcss";

  body {
    @apply text-base;
  }
  header {
    @apply leading-normal max-h-screen lg:flex lg:place-items-center lg:pr-[calc(var(--section-gap)_/_2)];
  }
  main {
    @apply w-[85%] max-w-[85%] text-left mx-auto pt-20;
    justify-content: left;
  }
  .nav {
    @apply flex fixed justify-center text-base text-center bg-[color:var(--color-background)] z-[100] top-0 inset-x-0 w-full;
    /* Add background so content doesn't show through */
    /* Ensure nav stays on top */
  }
  nav a.router-link-exact-active {
    @apply text-[color:var(--color-text)] hover:bg-transparent;
  }
  nav a {
    @apply text-[color:var(--color-text-navigation)] font-[bold] inline-block border-l-[color:var(--color-border)] px-4 py-0 border-l border-solid first-of-type:border-0;
  }
  @media (min-width: 1024px) {
    .logo {
      @apply ml-0 mr-8 my-0;
    }
    header .wrapper {
      @apply flex flex-wrap;
      place-items: flex-start;
    }
    nav {
      @apply text-left ml-[-1rem] text-base mt-4 px-0 py-4;
    }
  }
  .active-nav-item {
    @apply text-[color:var(--color-text)];
  }

  :deep(.p-menubar-item.p-focus .p-menubar-item-content) {
    background: transparent !important;
  }
  :deep(.p-menubar-item.p-focus:hover .p-menubar-item-content) {
    background: var(--p-menubar-item-focus-background) !important;
  }

</style>
