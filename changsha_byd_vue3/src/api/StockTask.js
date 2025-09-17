import request from '@/utils/http'
export function getPageList(params) {
  return request({
    url: '/wms/stockTask/Load',
    method: 'get',
    params
  })
}

export function getList(params) {
  return request({
    url: '/wms/stockTask/GetByQuery',
    method: 'get',
    params
  })
}

export function updatePriority(data) {
  return request({
    url: '/wms/stockTask/updatePriority',
    method: 'post',
    data
  })
}
export function updateTaskStatus(data) {
  return request({
    url: '/wms/stockTask/updateTaskStatus',
    method: 'post',
    data
  })
}
export function finishedTaskAndAddMoveTask(data){
	return request({
	  url: '/wms/stockTask/finishedTaskAndAddMoveTask',
	  method: 'post',
	  data
	})
}

export function addInStockTask(data) {
  return request({
    url: '/wms/stockTask/addInStockTask',
    method: 'post',
    data
  })
}

export function addOutStockTask(data) {
  return request({
    url: '/wms/stockTask/AddOutStockTask',
    method: 'post',
    data
  })
}

export function addRemoveStockTask(data) {
  return request({
    url: '/wms/stockTask/AddRemoveStockTask',
    method: 'post',
    data
  })
}


export function update(data) {
  return request({
    url: '/wms/stockTask/Update',
    method: 'post',
    data
  })
}

export function del(data) {
  return request({
  url: '/wms/stockTask/Delete',
    method: 'post',
    data
  })
}
