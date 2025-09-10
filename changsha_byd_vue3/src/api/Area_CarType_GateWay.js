import request from '@/utils/http'
// 获取warehouse列表
export function getPageList(params) {
  return request({
    url: '/wms/Area_CarType_GateWay/Load',
    method: 'get',
    params
  })
}

export function getList(params) {
  return request({
    url: '/wms/Area_CarType_GateWay/GetByQuery',
    method: 'get',
    params
  })
}
//添加warehouse信息
export function add(data) {
  return request({
    url: '/wms/Area_CarType_GateWay/add',
    method: 'post',
    data
  })
}
//修改warehouse信息
export function update(data) {
  return request({
    url: '/wms/Area_CarType_GateWay/update',
    method: 'post',
    data
  })
}
//删除warehouse信息
export function del(data) {
  return request({
    url: '/wms/Area_CarType_GateWay/DeleteEntity',
    method: 'post',
    data
  })
}

