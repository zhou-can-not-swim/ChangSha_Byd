import request from '@/utils/http'

export function getPageList(params) {
  return request({
    url: '/wms/gateway/Load',
    method: 'get',
    params
  })
}
export function getList(params) {
  return request({
    url: '/wms/gateway/GetByQuery',
    method: 'get',
        params
  })
}

export function add(data) {
  return request({
    url: '/wms/gateway/Add',
    method: 'post',
    data
  })
}

export function update(data) {
  return request({
    url: '/wms/gateway/Update',
    method: 'post',
    data
  })
}

export function del(data) {
  return request({
    url: '/wms/gateway/Delete',
    method: 'post',
    data
  })
}
