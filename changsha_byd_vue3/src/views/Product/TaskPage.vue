<template>
	<!-- 	作业管理 Task -->
	<div class="common-layout">
		<el-container>
			<el-header>
				<el-select v-model="selectedPriority" placeholder="选择优先级" style="width: 240px">
					<el-option v-for="item in priorityOptions" :key="item.key" :label="item.display_name" :value="item.key" />
				</el-select>
				<el-button type="primary" @click="UpdatePriority">更新优先级</el-button>

				<el-select v-model="seltaskStatus" placeholder="选择作业状态" style="width: 240px">
					<el-option v-for="item in taskStatusOptions" :key="item.key" :label="item.display_name" :value="item.key" />
				</el-select>
				<el-button type="primary" @click="UpdateTaskStatus">更新作业状态</el-button>
			</el-header>
			<el-main>
				<el-table :data="tableData" style="width: 100%" row-key="id" @selection-change="handleSelectionChange"
					ref="multipleTableRef">	

					<el-table-column type="selection" width="55" />
					<el-table-column property="taskNo" label="任务号" width="120" />
					<el-table-column property="priority" label="优先级" width="120" align="center">	
						<template #default="scope">
							<span :style="{'color': priorityOptions.find(u=>u.key==scope.row.priority)?.color}">
								{{ priorityOptions.find(u=>u.key==scope.row.priority)?.display_name}}
							</span>
						</template>
					</el-table-column>
					<el-table-column property="carTypeNum" label="RFID" width="120" />
					<el-table-column property="jxCarTypeNum" label="十进制RFID" width="120" />
					<el-table-column property="taskType" label="作业类型" width="120" align="center">
						<template #default="scope">
							<el-tag :type="taskTypeOptions.find(u => u.key == scope.row.taskType).color"
								disable-transitions>
								{{taskTypeOptions?.find(u => u.key == scope.row.taskType)?.display_name || "未知"}}
							</el-tag>
						</template>
					</el-table-column>
					<el-table-column property="taskStatus" label="作业状态" width="120" align="center">
						<template #default="scope">
							<span v-if="scope.row.taskStatus != null && scope.row.taskStatus != undefined">
								{{taskStatusOptions?.find(u => u.key ==
									scope.row.taskStatus)?.display_name || "未知"}}
							</span>
						</template>
					</el-table-column>
					<el-table-column property="gatewayName" label="出入口" width="120" />
				</el-table>
			</el-main>
		</el-container>
	</div>


</template>

<script setup>
import { onMounted, ref, watch } from 'vue';
import * as stockTasks from "@/api/StockTask";
const tableData = ref([
])

// 变量
const priorityOptions = ref([{
	key: 0,
	display_name: "普通",
	color: 'orange'
},
{
	key: 1,
	display_name: "优先",
	color: "red"
}
])

const taskTypeOptions = ref([{
	key: 1,
	display_name: "入库",
	color: "success"
},
{
	key: 2,
	display_name: "出库",
	color: "primary",
},
{
	key: 3,
	display_name: "移库",
	color: "warning"
},
{
	key: 4,
	display_name: "存车修正",
	color: "danger"
}
])

const taskStatusOptions = ref([{
	key: 0,
	display_name: "初始化"
}, {
	key: 1,
	display_name: "等待执行"
}, {
	key: 2,
	display_name: "正在执行"
}, {
	key: 3,
	display_name: "任务完成"
}, {
	key: 4,
	display_name: "故障"
}, {
	key: 5,
	display_name: "暂停"
}, {
	key: 6,
	display_name: "已校验等待出库"
}, {
	key: 7,
	display_name: "校验RFID不通过"
}, {
	key: 9,
	display_name: "取消任务"
}])

//表格多选

const multipleTableRef = ref()
const multipleSelection = ref([])
const handleSelectionChange = (val) => {
	multipleSelection.value = val
	//   console.log(multipleSelection.value)
}

//优先级
const selectedPriority = ref()
const seltaskStatus=ref()

//作业状态
const selectedTaskStatus=ref()
const getTaskList = () => {
	stockTasks.getList().then(response => {
		tableData.value = response.result;
	}).catch(error => { console.log(error) })
}
// 更新优先级
const UpdatePriority = () => {
	if (multipleSelection.value.length < 1) {
		$message({
			message: "请先选择你要更新的任务",
			type: "error"
		});
		return;
	}
	var selectids = multipleSelection.value.map(u => u.id);
	var param = {
		selectIds: selectids,
		priority: selectedPriority.value

	}
	stockTasks.updatePriority(param).then(response => {
		getTaskList()
		multipleSelection.value = []
		// this.$notify({
		// 	title: "成功",
		// 	message: "更新成功",
		// 	type: "success",
		// 	duration: 2000
		// });
	}).catch(error => {
		console.log("UpdatePriority-->", error);

	})
	
}
const UpdateTaskStatus = () => {
	if (multipleSelection.value.length < 1) {
		// $message({
		// 	message: "请先选择你要更新的任务",
		// 	type: "error"
		// });
		return;
	}
	var selectids = multipleSelection.value.map(u => u.id);
	var param = {
		selectIds: selectids,
		taskStatus: seltaskStatus.value

	}
	console.log("updateTaskStatus--->",param);
	stockTasks.updateTaskStatus(param).then(response => {
		
		
		getTaskList()
		multipleSelection.value = []
		// $notify({
		// 	title: "成功",
		// 	message: "更新成功",
		// 	type: "success",
		// 	duration: 2000
		// });
	}).catch(error => {
		console.log("UpdateTaskStatus-->", error);
	});
}
const FinishedTaskAndAddMoveTask = () => {
	if (this.multipleSelection.length != 1) {
		this.$message({
			message: "请先选择一条要退回的任务",
			type: "error"
		});
		return;
	}
	var selectid = this.multipleSelection.map(u => u.id)[0];
	var param = {
		taskId: selectid

	}
	stockTasks.finishedTaskAndAddMoveTask(param).then(response => {
		this.getList()
		this.multipleSelection = []
		this.$notify({
			title: "成功",
			message: "操作成功",
			type: "success",
			duration: 2000
		});
	}).catch(error => {
		console.log("FinishedTaskAndAddMoveTask-->", error);
	});
}




onMounted(() => {
	getTaskList()
})

</script>

<style scoped></style>