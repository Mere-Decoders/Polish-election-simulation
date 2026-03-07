import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import { useAuth } from '@/auth/useAuth'

declare module 'vue-router' {
  interface RouteMeta {
    requiresAuth?: boolean
  }
}

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/about',
      name: 'about',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/AboutView.vue'),
    },
    {
      path: '/simulation',
      name: 'simulation',
      component: () => import('../views/SimulationView.vue'),
      meta: {
        requiresAuth: true,
      },
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('../views/LoginView.vue'),
    },
    {
      path: '/logout',
      name: 'logout',
      redirect: () => {
        const auth = useAuth()
        auth.logout()
        return { name: 'login' }
      },
    },
  ],
})

router.beforeEach(async (to) => {
  if (!to.meta.requiresAuth) {
    return true
  }

  const auth = useAuth()
  const hasActiveSession = await auth.ensureSession()
  if (hasActiveSession) {
    return true
  }

  return {
    name: 'login',
    query: {
      redirect: to.fullPath,
    },
  }
})

export default router
