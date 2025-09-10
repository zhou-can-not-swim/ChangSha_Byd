import request from '@/utils/request'


export function loadUnitTypeList(params) {
  return request({
    url: '/wms/unit/LoadUnitType',
    method: 'get',
    params
  })
}

export function loadUnitByType(params){
  return request({
    url: '/wms/unit/LoadUnitByType',
    method: 'get',
    params
  })
}

export function addUnitType(data){
  return request({
    url: '/wms/unit/AddUnitType',
    method: 'post',
    data
  })
}

export function addUnit(data){
  return request({
    url: '/wms/unit/AddUnit',
    method: 'post',
    data
  })
}

export function deleteUnitType(data){
  return request({
    url: '/wms/unit/DeleteUnitType',
    method: 'post',
    data
  })
}

export function deleteUnit(data){
  return request({
    url: '/wms/unit/DeleteUnit',
    method: 'post',
    data
  })
}

export function updateUnitType(data){
  debugger
  return request({
    url: '/wms/unit/UpdateUnitType',
    method: 'post',
    data
  })
}

export function updateUnit(data){
  debugger
  return request({
    url: '/wms/unit/UpdateUnit',
    method: 'post',
    data
  })
}


//提供给下拉框使用
export function getTopUnitList(params) {
  return request({
    url: '/wms/unit/GetTopUnitList',
    method: 'get',
    params
  })
}