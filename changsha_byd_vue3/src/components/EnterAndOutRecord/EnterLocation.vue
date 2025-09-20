<template>
    <!-- 手动入库 -->
    <el-row>
        <el-col :span="12">
            <el-row>
                <el-col :span="4">
                    台车编号:
                </el-col>
                <el-col :span="20"><el-input v-model="inTemp.carTypeNum" style="width: 240px"
                        placeholder="请输入车编号" /></el-col>
            </el-row>

        </el-col>
        <el-col :span="12">
            <el-row>
                <el-col :span="4">
                    入库库口号:
                </el-col>
                <el-col :span="20">
                    <el-select v-model="inTemp.gatewayId" placeholder="选择入库库口号" style="width: 240px">
                        <el-option v-for="item in inGatewayOptions" :key="item.key" :label="item.display_name"
                            :value="item.key" />
                    </el-select>
                </el-col>
            </el-row>
        </el-col>
    </el-row>
    <el-row>
        <el-col :span="12">
            <el-row>
                <el-col :span="4">
                    库位排列层
                </el-col>
                <el-col :span="20">
                    {{ locationCode }}
                    <el-button type="primary" @click="openLocation">选择库位</el-button>
                </el-col>
            </el-row>
        </el-col>

        <el-col :span="12">
            <el-row>
                <el-col :span="4">
                    <div class="col-v-center">
                        维修入库：
                    </div>
                </el-col>
                <el-col :span="16">
                    <el-switch v-model="inTemp.isRepair" size="large"
                        style="--el-switch-on-color: #13ce66; --el-switch-off-color: #ff4949" active-text="Open"
                        inactive-text="Close" />
                </el-col>
                <el-col :span="4">
                    <el-button type="primary" @click="addInTask">添加入库作业</el-button>
                </el-col>
            </el-row>
        </el-col>
    </el-row>

    <el-dialog v-model="dialogTableVisible" title="选择库位" width="800px">
        <el-row>
            <el-col :span="10">
                <el-select ref="selarea" clearable filterable size="mini" class="filter-item" style="width: 200px"
                    v-model="listQuery.areaId" :placeholder="'选择库区'">
                    <el-option value label>请选择</el-option>
                    <el-option v-for="item in areaOptions" :key="item.key" :label="item.display_name"
                        :value="item.key"></el-option>
                </el-select>
            </el-col>
            <el-col :span="10">
                <el-input placeholder="库位编号" v-model="listQuery.key" style="width: 100px;">
                </el-input>
            </el-col>

            <el-col :span="4">
                <el-button type="primary" size="mini" @click="handleFilter">查询</el-button>
            </el-col>
        </el-row>


        <el-table :data="list" @click="handleClick" @current-change="handleCurrentChange">
            <el-table-column :show-overflow-tooltip="true" min-width="120px" :label="'编号排列层'" prop="code" sortable>

            </el-table-column>

            <el-table-column :show-overflow-tooltip="true" min-width="120px" :label="'所属仓库'" prop="warehouseId"
                sortable>
                <template #default="scope">
                    <span v-if="scope.row.warehouseId != null && scope.row.warehouseId != undefined">
                        {{warehouseOptions?.find(u => u.key ==
                            scope.row.warehouseId)?.display_name || "未知"}}
                    </span>
                </template>
            </el-table-column>
            <el-table-column :show-overflow-tooltip="true" min-width="120px" :label="'所属库区'" prop="areaId" sortable>

                <template #default="scope">
                    <span v-if="scope.row.areaId != null && scope.row.areaId != undefined">
                        {{areaOptions?.find(u => u.key ==
                            scope.row.areaId)?.display_name || "未知"}}
                    </span>
                </template>
            </el-table-column>


        </el-table>
                    <!-- 分页 -->
               <div class="demo-pagination-block">
    <el-pagination
      v-model:current-page="currentPage"
      :size="listQuery.limit"
      :disabled="disabled"
      :background="background"
      layout="total, prev, pager, next, jumper"
      :total="total"
      @current-change="handleCurrentChangePage"
    />
  </div>
    </el-dialog>

</template>


<script setup>
import { onMounted, ref } from 'vue'
import * as locations from '@/api/Location'
import * as stockTasks from '@/api/StockTask'
import { ElMessage } from 'element-plus'
import * as gateways from '@/api/Gateway'
import * as ToArray from '@/utils/convertArrayToKeyValueUnique'

// 分页变量
import  { ComponentSize } from 'element-plus'




const LocationNum = ref('')
const list = ref([])
const listQuery = ref({ // 查询条件
    page: 1,
    limit: 10,
    key: undefined,
    status: 0,
    areaId: undefined,
    isDisabled: false,
    isKukou: false,

})
const dialogTableVisible = ref(true)

const total = ref(0)
const inTemp = ref({
    carTypeNum: '',
    gatewayId: undefined,
    locationId: undefined

})
const warehouseOptions = ref([{
    key: 0,
    display_name: "无"
}, {
    key: 1,
    display_name: "待定"
}, {
    key: 2,
    display_name: "地板库"
}])
const areaOptions = ref([{
    key: 1,
    display_name: "EC区域"
}, {
    key: 2,
    display_name: "RF区域"
}, {
    key: 3,
    display_name: "FF区域"
}, {
    key: 4,
    display_name: "RSM区域"
}])


//打开库位
const dialogSelectLocation = ref(true)
const openLocation = () => {
    dialogTableVisible.value = true;
}
const getList = () => {

    locations.getPageList(listQuery.value).then(response => {
        list.value = response.data;
        total.value = response.count;
    });
}
//点击行
const locationCode = ref('')
const handleCurrentChange = (val) => {
    inTemp.value.locationId = val.id;
    locationCode.value = val.code;
    
    inTemp.value.locationId = val.id;
    dialogTableVisible.value = false;
    console.log(inTemp.value);
    
}
// 添加入库作业
const addInTask = () => {

    stockTasks.addInStockTask(inTemp.value)
        .then(response => {

            if (response.code == 200) {
                ElMessage({
                    message: response.message,
                    type: 'success',
                })
            } else {
                ElMessage({
                    message: response.message,
                    type: 'error',
                })
            }

        })

        .catch(error => {
            ElMessage({
                message: "未知错误：" + error,
                type: 'error',
            })
        });
}

const inGatewayOptions = ref([])
const getInGateway = () => {
    var params = {
        equipmentId: undefined,
        type: 1
    }
    gateways.getList(params).then(response => {

        inGatewayOptions.value = ToArray.convertToKeyValueArray(response.result)


    })

}
// 分页
const currentPage = ref(4)
const size = ref<ComponentSize>('default')
const background = ref(false)
const disabled = ref(false)

//第几页
const handleCurrentChangePage = (val) => {
    listQuery.value.page = val
    getList()
}


onMounted(() => {
    getList();
    getInGateway()
});

</script>

<style scoped>
.el-row {
    margin-bottom: 20px;
}

.el-row:last-child {
    margin-bottom: 0;
}

.el-col {
    border-radius: 4px;
}

.grid-content {
    border-radius: 4px;
    min-height: 36px;
}

.col-v-center {
    display: flex;
    align-items: center;
}

.demo-pagination-block + .demo-pagination-block {
  margin-top: 10px;
}
.demo-pagination-block .demonstration {
  margin-bottom: 16px;
}
</style>