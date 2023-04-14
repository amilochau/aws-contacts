import type { RouteRecordRaw } from 'vue-router';

const routes: Array<RouteRecordRaw> = [
  { name: 'Home', path: '', component: () => import('@/pages/home/PageHome.vue') },
  { name: 'Message', path: 'messages/:id', component: () => import('@/pages/message/PageMessage.vue') },
]

export default routes
