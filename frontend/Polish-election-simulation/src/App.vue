<template>
  <Menubar :model="items" class="nav">
    <template #item="{ item, props }">
      <router-link 
        v-if="item.route" 
        v-slot="{ href, navigate, isExactActive }" 
        :to="item.route" 
        custom
      >
        <a 
          :href="href" 
          v-bind="props.action" 
          @click="navigate" 
          :class="['nav-link', { 'active-nav-item': isExactActive }]"
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

/* bazowy stan */
.nav-link {
  @apply px-3 py-1 rounded-md transition-all duration-200 text-[color:var(--color-text-muted)];
}

.nav-link:hover {
  background: color-mix(in srgb, var(--color-text) 10%, transparent);
}

.active-nav-item {
  background: color-mix(in srgb, var(--color-text) 15%, transparent);
  @apply text-[color:var(--color-text)];
}

/* underline animation */
:deep(.p-menubar-item-content) {
  position: relative;
}

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

:deep(.p-menubar-item:hover .p-menubar-item-content::after) {
  width: 60%;
}

:deep(.p-menubar-item:has(.active-nav-item) .p-menubar-item-content::after) {
  width: 100%;
}

/* usuń podświetlenie (tło) aktywnego elementu */
:deep(.p-menubar-item.p-highlight > .p-menubar-item-content),
:deep(.p-menubar-item.p-focus > .p-menubar-item-content),
:deep(.p-menubar-item-content:focus),
:deep(.p-menubar-item-content:active) {
  background: transparent !important;
}

:deep(.p-menubar-root-list > .p-menubar-item:first-child.p-highlight > .p-menubar-item-content) {
  background: transparent !important;
}

:deep(.p-menubar-item.p-highlight) {
  background: transparent !important;
}

:deep(.p-menubar-item-content:hover) {
  background: transparent !important;
}
</style>
