import request from '@/utils/request'
// 获取warehouse列表
export function getCarTypePageList(params) {
  return request({
    url: '/wms/carType/Load',
    method: 'get',
    params
  })
}

export function getCarTypeList(params) {
  return request({
    url: '/wms/carType/GetByQuery',
    method: 'get',
    params
  })
}
//添加warehouse信息
export function add(data) {
  return request({
    url: '/wms/carType/add',
    method: 'post',
    data
  })
}
//修改warehouse信息
export function update(data) {
  return request({
    url: '/wms/carType/update',
    method: 'post',
    data
  })
}
//删除warehouse信息
export function del(data) {
  return request({
    url: '/wms/carType/DeleteEntity',
    method: 'post',
    data
  })
}

