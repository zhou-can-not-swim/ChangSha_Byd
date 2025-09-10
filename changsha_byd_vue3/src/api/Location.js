import request from '@/utils/request'

export function getPageList(params) {
  return request({
    url: '/wms/location/Load',
    method: 'get',
    params
  })
}
export function getList(params) {
  return request({
    url: '/wms/location/GetByQuery',
    method: 'get',
        params
  })
}

export function add(data) {
  return request({
    url: '/wms/location/Add',
    method: 'post',
    data
  })
}

export function update(data) {
  return request({
    url: '/wms/location/Update',
    method: 'post',
    data
  })
}

export function del(data) {
  return request({
    url: '/wms/location/deleteEntity',
    method: 'post',
    data
  })
}
//获取该库位的操作记录
export function getOptionRecordPageList(params) {
  return request({
    url: '/wms/stockRecord/Load',
    method: 'get',
    params
  })
}

export function initLocation(data) {
  return request({
    url: '/wms/location/InitLocation',
    method: 'post',
    data
  })
}
export function initLocationArea(data)
{
	
	return request({
    url: '/wms/location/InitLocationArea',
    method: 'post',
    data
  })
}
//下拉框选择
export function getTopLocationList(params) {
  return request({
    url: '/wms/location/GetTopLocationList',
    method: 'get',
    params
  })
}

//看板使用
export function getBoard(params) {
  return request({
    url: '/wms/location/GetBoard',
    method: 'get',
    params
  })
}

export function GetLocataionCount() {
  return request({
    url: '/wms/location/GetLocataionCount',
    method: 'get'
  })
}
//只查询是库口的
export function getKukou(params) {
  return request({
    url: '/wms/location/GetKukou',
    method: 'get',
    params
  })
}

export function getFloor(params) {
  return request({
    url: 'wms/Location/GetLocation',
    method: 'get',
    params
  })
}



