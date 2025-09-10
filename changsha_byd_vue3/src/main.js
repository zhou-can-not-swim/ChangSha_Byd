import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'

// 添加element-plus
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
//引入全局
import '@/assets/global.css'

var app = createApp(App)

app.use(ElementPlus)

app.use(store)
app.use(router)
app.mount('#app')
