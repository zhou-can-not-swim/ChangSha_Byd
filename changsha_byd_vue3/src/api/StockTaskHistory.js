import request from '@/utils/request'
export function getPageList(params) {
  return request({
    url: '/wms/stockTaskHistory/Load',
    method: 'get',
    params
  })
}

export function GetTongJiTaskHistroy()
{
	return request({
	  url: '/wms/stockTaskHistory/GetTongJiTaskHistroy',
	  method: 'get'
	})
	
}

export function GetTongJiTaskDayHistroy()
{
	return request({
	  url: '/wms/stockTaskHistory/GetTongJiTaskDayHistroy',
	  method: 'get'
	})
	
}