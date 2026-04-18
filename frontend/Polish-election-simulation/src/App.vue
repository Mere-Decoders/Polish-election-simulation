<template>
  <!-- Main navigation bar using PrimeVue -->
  <Menubar :model="items" class="nav">   
    <!-- Custom rendering for each menu item -->
    <template #item="{ item, props }">
      <router-link 
        v-if="item.route" 
        v-slot="{ href, navigate, isActive }" 
        :to="item.route" 
        custom
      >
        <a 
          :href="href" 
          v-bind="props.action" 
          @click="navigate" 
          :class="['nav-link', { 'active-nav-item': isActive }]"
        >
          <span>{{ item.label }}</span>
        </a>
      </router-link>
      <a v-else :href="item.url" :target="item.target" v-bind="props.action">
        <span>{{ item.label }}</span>
      </a>
    </template>
  </Menubar>

  <!-- Page content gets rendered here -->
  <main>
    <RouterView />
  </main>
</template>

<script setup lang="ts">
import { RouterView } from 'vue-router'
import { computed } from 'vue'
import Menubar from 'primevue/menubar'
import { useAuth } from '@/auth/useAuth'

type NavItem = {
  label: string
  route?: string
  url?: string
  target?: string
}

const auth = useAuth()

const items = computed<NavItem[]>(() => [
  { label: 'Home', route: '/' },
  { label: 'About', route: '/about' },
  { label: 'Election simulation', route: '/simulation' },
  auth.isAuthenticated.value
    ? { label: 'Logout', route: '/logout' }
    : { label: 'Login', route: '/login' }
])
</script>

<style scoped>
@reference "tailwindcss";

.nav {
  @apply fixed top-0 inset-x-0 w-full flex justify-center text-base bg-[color:var(--color-background)] z-[100];
}

main {
  @apply w-[85%] mx-auto pt-20 text-left;
}

.nav-link {
  @apply px-3 py-1 rounded-md transition-all duration-200 text-[color:var(--color-text-muted)];
}

/*
  Hover state:
  - subtle background highlight (custom instead of PrimeVue default)
*/
.nav-link:hover {
  background: color-mix(in srgb, var(--color-text) 10%, transparent);
}

/*
  Active navigation item:
  - stronger background
  - full text color
*/
.active-nav-item {
  background: color-mix(in srgb, var(--color-text) 15%, transparent);
  @apply text-[color:var(--color-text)];
}

:deep(.p-menubar-item-content) {
  position: relative;
}

/*
  Underline (initial state):
  - centered horizontally
  - width 0 → grows via animation
*/
:deep(.p-menubar-item-content::after) {
  content: "";
  position: absolute;
  left: 50%;
  bottom: 0;
  width: 0%;
  height: 2px;
  background: var(--color-text);
  transform: translateX(-50%);
  transition: width 0.25s ease;
}

/*
  Hover effect:
  - underline expands to 60%
*/
:deep(.p-menubar-item:hover .p-menubar-item-content::after) {
  width: 60%;
}

/*
  Active item:
  - underline expands to full width
  - detected via presence of .active-nav-item
*/
:deep(.p-menubar-item:has(.active-nav-item) .p-menubar-item-content::after) {
  width: 100%;
}

/*
  Remove default PrimeVue background styles:
  - prevents unwanted highlight/focus background
  - ensures full visual control
*/
:deep(.p-menubar-item-content) {
  background: transparent !important;
}

</style>
