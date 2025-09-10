import request from '@/utils/request'
// 获取warehouse列表
export function getAreaPageList(params) {
  return request({
    url: '/wms/area/Load',
    method: 'get',
    params
  })
}

export function getAreaList(params) {
  return request({
    url: '/wms/area/GetByQuery',
    method: 'get',
    params
  })
}
//添加warehouse信息
export function add(data) {
  return request({
    url: '/wms/area/add',
    method: 'post',
    data
  })
}
//修改warehouse信息
export function update(data) {
  return request({
    url: '/wms/area/update',
    method: 'post',
    data
  })
}
//删除warehouse信息
export function del(data) {
  return request({
    url: '/wms/area/DeleteEntity',
    method: 'post',
    data
  })
}

