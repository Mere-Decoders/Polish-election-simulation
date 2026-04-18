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
          @click="navigate(); ($event.target as HTMLElement).blur()" 
          :class="{ 'active-nav-item': isExactActive }"
        >
          <span>{{ item.label }}</span>
        </a>
      </router-link>

      <a 
        v-else 
        :href="item.url" 
        :target="item.target" 
        v-bind="props.action"
      >
        <span>{{ item.label }}</span>
      </a>
    </template>
  </Menubar>

  <main>
    <RouterView v-slot="{ Component }">
      <Transition name="fade-slide" mode="out-in">
        <component :is="Component" />
      </Transition>
    </RouterView>
  </main>
</template>

<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
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

main {
  @apply w-[85%] mx-auto pt-20;
}

.nav {
  @apply fixed top-0 inset-x-0 flex justify-center z-[100] w-full bg-[color:var(--color-background)];
}

/* aktywny link */
.active-nav-item {
  @apply text-[color:var(--color-text)];
}

/* usuwa focus background z PrimeVue */
:deep(.p-menubar-item.p-focus .p-menubar-item-content) {
  background: transparent !important;
}

:deep(.p-menubar-item.p-focus:hover .p-menubar-item-content) {
  background: var(--p-menubar-item-focus-background) !important;
}

/* aktywne tło */
:deep(.p-menubar-item:has(.active-nav-item) .p-menubar-item-content) {
  background: var(--p-menubar-item-focus-background);
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
</style>