import request from '@/utils/request'
// 获取warehouse列表
export function getList(params) {
  return request({
    url: '/wms/ProductionLog/Load',
    method: 'get',
    params
  })
}