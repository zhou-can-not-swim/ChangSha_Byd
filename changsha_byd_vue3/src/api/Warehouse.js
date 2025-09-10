import request from '@/utils/request'
// 获取warehouse列表
export function getWarehousesPageList(params) {
  return request({
    url: '/wms/warehouse/loadWarehouse',
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

//获取仓库
export function getWarehouseList(params) {
  return request({
    url: '/wms/warehouse/getByQuery',
    method: 'get',
        params
  })
}



//添加warehouse信息
export function add(data) {
  return request({
    url: '/wms/warehouse/addWarehouse',
    method: 'post',
    data
  })
}
//修改warehouse信息
export function update(data) {
  return request({
    url: '/wms/warehouse/updateWarehouse',
    method: 'post',
    data
  })
}
//删除warehouse信息
export function del(data) {
  return request({
    url: '/wms/warehouse/deleteWarehouse',
    method: 'post',
    data
  })
}


