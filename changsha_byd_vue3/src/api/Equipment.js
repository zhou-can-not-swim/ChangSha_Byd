import request from '@/utils/request'

export function getPageList(params) {
  return request({
    url: '/wms/equipment/load',
    method: 'get',
    params
  })
}


export function getList(params) {
  return request({
    url: '/wms/equipment/getByQuery',
    method: 'get',
        params
  })
}


export function add(data) {
  return request({
    url: '/wms/equipment/Add',
    method: 'post',
    data
  })
}

export function update(data) {
  return request({
    url: '/wms/equipment/Update',
    method: 'post',
    data
  })
}

export function del(data) {
  return request({
    url: '/wms/equipment/Delete',
    method: 'post',
    data
  })
}
