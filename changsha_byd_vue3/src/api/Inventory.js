import request from '@/utils/http'

export function load(params) {
  return request({
    url: '/wms/Inventory/Load',
    method: 'get',
    params
  })
}
//查询库存
export function getByQuery(params) {
  return request({
    url: '/wms/Inventory/GetByQuery',
    method: 'get',
    params
  })
}

//查询库存
export function getInventoryList(params) {
  return request({
    url: '/wms/Inventory/GetInventoryList',
    method: 'get',
    params
  })
}

export function getBoardCe(params) {
  return request({
    url: '/wms/Inventory/GetBoardCe',
    method: 'get',
    params
  })
}

export function getInventoryEntity(params) {
  return request({
    url: '/wms/Inventory/GetInventoryEntity',
    method: 'get',
    params
  })
}

export function GetInventoryReport() {
  return request({
    url: '/wms/Inventory/GetInventoryReport',
    method: 'get'
  })
}