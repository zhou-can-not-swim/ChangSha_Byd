import axios from 'axios'

// 创建 axios 实例
const service = axios.create({
  // :5002/api
  baseURL: process.env.VUE_APP_BASE_API, // 从环境变量获取基础 URL
  timeout: 10000, // 请求超时时间
  headers: { 'Content-Type': 'application/json;charset=utf-8' }
})

// 请求拦截器
service.interceptors.request.use(
  (config) => {
     // 在发送请求之前做些什么，例如添加 token
    // const token = localStorage.getItem('token')
    // if (token) {
    //   // 正确设置Authorization头
    //   config.headers.Authorization = token
    // }
    // 移除自动跳转到登录页的逻辑，让请求可以正常发送
    return config
  },error => {
    // 对请求错误做些什么
    console.log(error)
    alert("http.js-->请求失败:"+error)
  }
)

// 响应拦截器
service.interceptors.response.use(
  (response) => {
    if(response.status != 200){
      alert("http.js-->响应失败:"+response.status)
    }else{
      return response.data
    }
  }
)

export default service