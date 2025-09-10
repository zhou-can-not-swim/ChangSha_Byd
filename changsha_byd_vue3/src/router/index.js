import { createRouter, createWebHashHistory } from 'vue-router'

const routes = [
  {
    path: '/',
    name: 'layout',
    component: () => import('../views/Layout.vue'),
    children: [
      {
        path: '/',
        name: 'HomePage',
        component: () => import('../views/HomePage.vue')
      },
      {
        path: '/wms/task',
        name: 'task',
        component: () => import('../views/Product/TaskPage.vue')
      }, {
        path: '/wms/createTask',
        name: 'createTask',
        component: () => import('../views/Product/EnterAndOutRecord.vue')
      }, {
        path: '/wms/kucun',
        name: 'kucun',
        component: () => import('../views/Product/InventoryPage.vue')
      }, {
        path: '/wms/taskRecord',
        name: 'taskRecord',
        component: () => import('../views/Product/EnterAndOutRecord.vue')
      }, {
        path: '/wms/enterAndOutRuleSetting',
        name: 'enterAndOutRuleSetting',
        component: () => import('../views/Product/EnterAndOutRuleSetting.vue')
      }, {
        path: '/wms/carType',
        name: 'carType',
        component: () => import('../views/Equipment/CarTypePage.vue')
      }
    ]
  }
]

const router = createRouter({
  history: createWebHashHistory(),
  routes
})

export default router
