<template>
    <div>
        <el-card class="box-card right-card">
            <!-- <button @click="connect">连接</button>
            <button @click="disconnect" >断开连接</button> -->
            <p>plc心跳响应<input v-model="context_plc.heartBeatAck" type="checkbox" :true-value="true"
                    :false-value="false">{{ context_plc.heartBeatAck }}</p>
            <p>plc心跳请求<input v-model="context_plc.heartBeatReq" type="checkbox" :true-value="true"
                    :false-value="false">{{ context_plc.heartBeatReq }}</p>

            <p>mst心跳响应<input v-model="context_mst.heartBeatAck" type="checkbox" :true-value="true"
                    :false-value="false">{{ context_mst.heartBeatAck }}</p>
            <p>mst心跳请求<input v-model="context_mst.heartBeatReq" type="checkbox" :true-value="true"
                    :false-value="false">{{ context_mst.heartBeatReq }}</p>


            <p>mst下发任务请求：<input v-model="context_mst.SendTaskReq" type="checkbox" :true-value="true"
                    :false-value="false">{{ context_mst.SendTaskReq }}</p>
            <p>plc下发任务确认：<input v-model="context_plc.SendTaskAck" type="checkbox" :true-value="true"
                    :false-value="false">{{ context_plc.SendTaskAck }}</p>

            <p>plc完成任务请求<input v-model="context_plc.FinishTaskReq" type="checkbox" :true-value="true"
                    :false-value="false">{{ context_plc.FinishTaskReq }}</p>
            <p>mst完成任务确认<input v-model="context_mst.FinishTaskAck" type="checkbox" :true-value="true"
                    :false-value="false">{{ context_mst.FinishTaskAck }}</p>

        </el-card>

    </div>
</template>

<script setup>
import service from "@/utils/http";
import axios from "axios";
import { watch, onMounted, onUnmounted, reactive, ref } from "vue";
// const context_temp = ref({})
const context_mst = ref({
    "EC010_A库口": {
        RequestTaskRFID: "0",
        StandByAck: false
    },
    "EC010_B库口": {
        RequestTaskRFID: "0",
        StandByAck: false
    },
    EndColumn: 0,
    EndFloor: 0,
    EndLine: 0,
    FinishTaskAck: false,
    RequsetOutTaskReq: false,
    RequsetTaskReq: false,
    RequsetTaskResultAck: false,
    SendTaskReq: false,
    StartColumn: 0,
    StartFloor: 0,
    StartLine: 0,
    TaskNo: 0,
    TaskRFID: 0,
    TaskType: 0,
    VerificationCode: 0,
    heartBeatAck: false,
    heartBeatReq: false
});

const context_plc = ref({
    CurrentColumn: 0,
    CurrentFloor: 0,
    CurrentLine: 0,
    DoTaskNo: 0,
    "EC010_A库口": {
        EntryRFID: "0",
        OutStatus: false,
        RequestTaskResult: 0,
        StandByReq: false
    },
    "EC010_B库口": {
        EntryRFID: "0",
        OutStatus: false,
        RequestTaskResult: 0,
        StandByReq: false
    },
    ErrorCode1: "",
    ErrorCode2: "",
    ErrorCode3: "",
    ErrorCode4: "",
    ErrorCode5: "",
    FinishTaskReq: false,
    RequestOutTaskAck: false,
    RequestTaskAck: false,
    RequestTaskResultReq: false,
    SendTaskAck: false,
    StockerAction: 0,
    StockerCargo: 0,
    StockerTrip: 0,
    StorkerStatus: 0,
    TaskNo: 0,
    VerificationCode: 0,
    heartBeatAck: false,
    heartBeatReq: false
})
// Watcher
// watch([context_mst.value,context_plc.value], ([newm, newp], [oldm, oldp]) => {
//     service.post('/api/db', newm.value,newm.value).then(res => {
//         console.log('Response from server:', res.data);
//     })
// }, { deep: true })


watch([context_plc, context_mst],
  ([newPlc, newMst]) => {
    service.post('/api/db', { plc: newPlc, mst: newMst })
           .then(res => console.log('saved', res.data));
  }, { deep: true });
// Lifecycle hooks
// onMounted(() => {
//     signalrs.startNewConnection('duiduoji')
//     window.RefreshPlcMsg = RefreshPlcMsg
//     signalrs.stopConnection()

// })

// // 手动控制连接的方法
// const connect = () => {
//     signalrs.startNewConnection('duiduoji')
//     window.RefreshPlcMsg = RefreshPlcMsg
// }

// const disconnect = () => {
//     signalrs.stopConnection()
// }

// const RefreshPlcMsg = (data, no) => {
//     if (no == 1) {
//     } else if (no == 2) {
//         context_temp.value = data;
//         setPlcData(data.PlcInfo)

//         console.log('QH data updated:', data);
//     }
// }

// const setPlcData = (PlcInfo) => { 
//     console.log('PlcInfo:', PlcInfo);
// }
</script>


<style scoped>
.text {
    font-size: 14px;
}

.item {
    margin-bottom: 18px;
}

.clearfix:before,
.clearfix:after {
    display: table;
    content: "";
}

.clearfix:after {
    clear: both
}

.box-card {
    width: 50%;
}

.left-card {
    float: left;
}

.right-card {
    float: right;
}

/* 通用样式 */
.custom-theme .el-card__body {
    padding: 5px;
}


.custom-theme .el-card__header {

    background-color: #a235f5;
    color: #fff;
}

.custom-theme .el-card.is-always-shadow {
    box-shadow: 2px 2px 7px 5px rgb(191 157 206 / 50%);
}

.GreenBall {

    width: 12px;
    height: 12px;
    border-radius: 50%;

    background: radial-gradient(circle at 5px 5px, #00ff4e, #424040);

}

.RedBall {

    width: 12px;
    height: 12px;
    border-radius: 50%;
    background: radial-gradient(circle at 5px 5px, #f30101, #424040);

}

.GreyBall {
    width: 12px;
    height: 12px;
    border-radius: 50%;
    background: radial-gradient(circle at 5px 5px, #c9cbce, #bcb7b7);
}

.ssdiv {
    display: table-cell;
    padding-right: 1rem;


}

.ssdivText {
    display: table-cell;
    padding-right: 1rem;
}

.tc {
    display: table-cell;
}

.sstext {

    display: table-cell;
    height: 1rem;
    font-size: 0.8rem;
    vertical-align: middle;
}

.sstitle {
    height: 1rem;
    font-size: 0.8rem;
    /* 	vertical-align: middle;
		text-align: center; */
}

.sstext_color {
    color: #409eff;
}
</style>