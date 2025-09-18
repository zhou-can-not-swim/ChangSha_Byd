import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'

// 添加element-plus
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
//引入全局
import '@/assets/global.css'

// ResizeObserver 错误处理
if (typeof window !== 'undefined') {
  window.addEventListener('error', (e) => {
    if (e.message && e.message.includes('ResizeObserver')) {
      e.stopImmediatePropagation()
    }
  })
}




var app = createApp(App)


// 全局错误处理
app.config.errorHandler = (err, vm, info) => {
  if (err.message && err.message.includes('ResizeObserver')) {
    return false
  }
  console.error('Global error:', err)
}


app.use(ElementPlus)



app.use(store)
app.use(router)
app.mount('#app')
