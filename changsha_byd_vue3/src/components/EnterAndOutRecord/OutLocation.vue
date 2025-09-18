<template>

    <!-- 手动出库 -->
    台车编号：<el-input v-model="carTypeNum" style="width: 240px" placeholder="" />
    库区：<el-select v-model="kuqu" placeholder="选择库区" style="width: 240px">
        <el-option v-for="item in areaOptions" :key="item.value" :label="item.label" :value="item.value" />
    </el-select>
    车型：<el-input v-model="input" style="width: 240px" placeholder="Please input" />
    <el-button type="primary" size="small" @click="searchOutKuCun">查询</el-button>

    <el-divider content-position="left">操作</el-divider>

    维修出库：
    <el-switch v-model="listQuery.isRepair" size="large" active-text="开" inactive-text="关" />
    <el-button type="primary" size="small" @click="addOutTasks">将选择的台车添加出库任务</el-button>


    <!-- 列表 -->
    <el-table ref="KucunTable" :key="tableKey" :data="KuCunlist" row-key="id" border fit stripe
        highlight-current-row style="width: 100%;height: auto;" @selection-change="handleSelectionChange" align="left">
        <el-table-column type="selection" align="center" width="55">
        </el-table-column>
        <el-table-column :show-overflow-tooltip="true" min-width="140px" align="center" :label="'库区'"
            property="areaName" sortable>
        </el-table-column>
        <el-table-column :show-overflow-tooltip="true" min-width="120px" align="center" :label="'库位排-列-层'"
            prop="locationCode" sortable>
        </el-table-column>

        <el-table-column :show-overflow-tooltip="true" min-width="80px" align="center" :label="'台车编号RFID'"
            prop="carTypeNum" sortable>

        </el-table-column>
        <el-table-column :show-overflow-tooltip="true" min-width="80px" align="center" :label="'风车类型'" prop="carTypeInt"
            sortable v-if="show_Fengche">

        </el-table-column>
        <el-table-column :show-overflow-tooltip="true" min-width="80px" align="center" :label=textTitle
            prop="jxCarTypeNum" sortable>

        </el-table-column>
        <el-table-column :show-overflow-tooltip="true" min-width="180px" align="center" :label="'车型'" prop="carTypeName"
            sortable>
        </el-table-column>
        <el-table-column :show-overflow-tooltip="true" min-width="180px" align="center" :label="'入库时间'"
            prop="createTime" sortable>
        </el-table-column>

    </el-table>

</template>


<script setup>
import { onMounted, ref } from 'vue'
import * as inventorys from '@/api/Inventory'
import * as stockTasks from '@/api/StockTask'

const carTypeNum = ref('')
const areaOptions = ref([])
const KuCunlist = ref([])
const listQuery = ref({
    page: 1,
    limit: 20,
    key: undefined,
    carTypeNum: '',
    carTypeId: undefined,
    outGatewayId: undefined,
    isRepair: false
})
//查询出库库存
const searchOutKuCun = () => {
    inventorys.getByQuery(listQuery.value).then(response => {
        KuCunlist.value = response.result;
    });
}

//多选
const multipleTableRef = ref()
const multipleSelection = ref([])
const handleSelectionChange = (val) => {
    multipleSelection.value = val
    console.log(multipleSelection.value)
}

const getKuCuList = () => {
    // if (WarehouseId != 1) {
    // 	// show_Hidder=false;
    // 	textTitle = "十进制RFID";
    // } else {
    // 	textTitle = "机械编号";
    // }
    inventorys.getByQuery(listQuery.value).then(response => {

        KuCunlist.value = response.result;

    });
}
const addOutTasks = () => {
    console.log("multipleSelection.value -->",multipleSelection.value);
    
    if (multipleSelection.value == undefined || multipleSelection.value.length < 1) {
        $message({
            message: "请先选择你要出库的库存数据",
            type: "error"
        });
        return;
    }
    var selectids = multipleSelection.value.map(u => u.id);
    var param = {
        inventoryIds: selectids,
        isRepair: listQuery.value.isRepair
    }
    console.log("param -->",param);
    
    stockTasks.addOutStockTask(param)
        .then(response => {

            if (response.code = 200) {
                getKuCuList();
                // $notify({
                //     title: "成功",
                //     message: "添加成功",
                //     type: "success",
                //     duration: 2000
                // });
                console.log("response-->",response);
                
            } else {

                // $notify({
                //     title: "失败",
                //     message: response.message,
                //     type: "error",
                //     duration: 2000
                // });
            }
        })
        .catch(error => {
            // $notify({
            //     title: "失败",
            //     message: "操作失败",
            //     type: "error",
            //     duration: 2000
            // });
        });



}

onMounted(() => {
    searchOutKuCun()
})
</script>

<style></style>