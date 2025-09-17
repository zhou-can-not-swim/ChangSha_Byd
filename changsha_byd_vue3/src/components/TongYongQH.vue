<template>
    <!-- QH调试界面 -->
    <div>
        <div class="text" type="flex" style="height: 1.5rem">
            <div class="ssdiv">
                <div v-if="PlcInfo?.heartBeatAck" class="GreenBall tc"> </div>
                <div v-else class="GreyBall tc"> </div>
                <div class="sstext">PLC心跳响应{{ typeof (PlcInfo?.heartBeatAck) }}</div>
            </div>
            <div class="ssdiv">
                <div v-if="context?.plcInfo?.heartBeatReq" class="GreenBall tc">
                </div>
                <div v-else class="GreyBall tc"> </div>
                <div class="sstext">PLC心跳请求
                </div>
            </div>

            <div class="ssdiv">
                <div v-if="context?.mstInfo?.heartBeatAck" class="GreenBall tc"> </div>
                <div v-else class="GreyBall tc"> </div>
                <div class="sstext">Mst心跳响应</div>
            </div>
            <div class="ssdiv">
                <div v-if="context?.mstInfo?.heartBeatReq" class="GreenBall tc">
                </div>
                <div v-else class="GreyBall tc"> </div>
                <div class="sstext">Mst心跳请求
                </div>
            </div>

            <div class="ssdiv">
                <div v-if="context?.mstInfo?.SendTaskReq" class="GreenBall tc"> </div>
                <div v-else class="GreyBall tc"> </div>
                <div class="sstext">下发任务请求</div>
            </div>
            <div class="ssdiv">
                <div v-if="context?.plcInfo?.SendTaskAck" class="GreenBall tc">
                </div>
                <div v-else class="GreyBall tc"> </div>
                <div class="sstext">下发任务确认
                </div>
            </div>

            <div class="ssdiv">
                <div v-if="context?.plcInfo?.finishTaskReq" class="GreenBall tc">
                </div>
                <div v-else class="GreyBall tc"> </div>
                <div class="sstext">完成任务请求
                </div>
            </div>
            <div class="ssdiv">
                <div v-if="context?.mstInfo?.finishTaskAck" class="GreenBall tc"> </div>
                <div v-else class="GreyBall tc"> </div>
                <div class="sstext">完成任务确认</div>
            </div>
        </div>
        <div class="text" type="flex" style="height: 1.5rem">
            <div class="ssdivText" style="font-weight: bold;">
                <div class="sstext">堆垛机实时状态</div>
            </div>
        </div>

        <div class="ssdivText">
            <div class="sstext">状态: </div><span class="sstext sstext_color" v-if="context != null">
                {{storkerStatusOptions.find(u => u.key == context?.plcInfo?.StorkerStatus)?.display_name}}

            </span>
        </div>

        <div class="ssdivText">
            <div class="sstext">行程: </div><span class="sstext sstext_color" v-if="context != null">
                {{stockerTripOptions.find(u => u.key ==
                    context?.plcInfo?.StockerTrip)?.display_name}}

            </span>
        </div>



        <div class="ssdivText">
            <div class="sstext">是否载货: </div><span class="sstext sstext_color" v-if="context != null">
                {{stockerCargoOptions.find(u => u.key ==
                    context.plcInfo.StockerCargo)?.display_name}}
            </span>
        </div>
        <div class="ssdivText">
            <div class="sstext">动作: </div><span class="sstext sstext_color" v-if="context != null">
                {{stockerActionOptions.find(u => u.key ==
                    context.plcInfo.StockerAction)?.display_name}}

            </span>
        </div>

        <div class="ssdivText">
            <div class="sstext">当前排列层: </div><span class="sstext sstext_color" v-if="context != null">
                {{ context?.plcInfo?.CurrentFloor + '-' + context?.plcInfo?.CurrentLine + '-' +
                    context?.plcInfo?.CurrentColumn }}
            </span>
        </div>





        <div class="ssdivText">
            <div class="sstext">执行包号: </div><span class="sstext sstext_color" v-if="context != null">
                {{ context?.plcInfo?.DoTaskNo }}
            </span>
        </div>
        <div class="ssdivText">
            <div class="sstext">完成包号: </div><span class="sstext sstext_color" v-if="context != null">
                {{ context?.plcInfo?.TaskNo }}
            </span>
        </div>
        <div class="ssdivText">
            <div class="sstext">下发任务校验结果: </div><span class="sstext sstext_color" v-if="context != null">
                {{requestTaskResultOptions.find(u => u.key ==
                    context?.plcInfo?.VerificationCode)?.display_name}}
            </span>
        </div>
    </div>
    <div class="text" type="flex" style="height: 1.5rem">
        <div class="ssdivText">
            <div class="sstext">报警1: </div><span class="sstext sstext_color" v-if="context != null">
                {{ context?.plcInfo?.ErrorCode1 }}
            </span>
        </div>
        <div class="ssdivText">
            <div class="sstext">报警2: </div><span class="sstext sstext_color" v-if="context != null">
                {{ context?.plcInfo?.ErrorCode2 }}
            </span>
        </div>

        <div class="ssdivText">
            <div class="sstext">报警3: </div><span class="sstext sstext_color" v-if="context != null">
                {{ context?.plcInfo?.ErrorCode3 }}

            </span>
        </div>

        <div class="ssdivText">
            <div class="sstext">报警4: </div><span class="sstext sstext_color" v-if="context != null">
                {{ context?.plcInfo?.ErrorCode4 }}

            </span>
        </div>

        <div class="ssdivText">
            <div class="sstext">报警5: </div><span class="sstext sstext_color" v-if="context != null">
                {{ context?.plcInfo?.ErrorCode5 }}
            </span>
        </div>
    </div>
    <div class="text" type="flex" style="height: 1.5rem">

        <div class="ssdivText" style="font-weight: bold;">
            <div class="sstext">上位机下发任务</div>
        </div>




        <div class="ssdiv">
            <div class="sstext">下发包号: </div><span class="sstext sstext_color" v-if="context != null">
                {{ context.mstInfo.TaskNo }}
            </span>
        </div>
        <div class="ssdivText">
            <div class="sstext">下发命令代码: </div><span class="sstext sstext_color" v-if="context != null">
                {{taskTypeOptions.find(u => u.key == context.mstInfo.TaskType)?.display_name}}
            </span>

        </div>



        <div class="ssdivText">
            <div class="sstext">下发取货排列层: </div><span class="sstext sstext_color" v-if="context != null">
                {{ context?.mstInfo?.StartLine + '-' + context?.mstInfo?.StartColumn + '-' +
                    context?.mstInfo?.StartFloor }}
            </span>
        </div>
        <div class="ssdivText">
            <div class="sstext">下发放货排列层: </div><span class="sstext sstext_color" v-if="context != null">
                {{ context?.mstInfo?.EndLine + '-' + context?.mstInfo?.EndColumn + '-' + context?.mstInfo?.EndFloor }}
            </span>
        </div>

        <div class="ssdivText">
            <div class="sstext">校验号: </div><span class="sstext sstext_color" v-if="context != null">
                {{ context?.mstInfo?.VerificationCode }}
            </span>
        </div>
        <div class="ssdivText">
            <div class="sstext">下发任务RFID: </div><span class="sstext sstext_color" v-if="context != null">
                {{ context?.mstInfo?.TaskRFID }}
            </span>
        </div>
    </div>

    <div class="text" type="flex" style="height: 1.5rem;margin-left: 300px;">

        <div class="sstitle" style="font-weight: bold;">机 舱 区 域
        </div>

    </div>

    <!-- EC010_A库口 -->
<div class="text" type="flex" style="height: 1.5rem">
    <div class="ssdivText" style="font-weight: bold;">
        <div class="sstext">EC010_A库口</div>
    </div>
    <div class="ssdiv">
        <div v-if="context != null && context?.plcInfo?.Gateways?.EC010_A库口?.StandByReq" class="GreenBall tc"></div>
        <div v-else class="GreyBall tc"></div>
        <div class="sstext">入库就位请求</div>
    </div>
    <div class="ssdiv">
        <div v-if="context != null && context?.mstInfo?.Gateways?.EC010_A库口?.StandByAck" class="GreenBall tc"></div>
        <div v-else class="GreyBall tc"></div>
        <div class="sstext">入库就位确认</div>
    </div>
    <div class="ssdiv">
        <div v-if="context != null && context?.plcInfo?.Gateways?.EC010_A库口?.OutStatus" class="GreenBall tc"></div>
        <div v-else class="GreyBall tc"></div>
        <div class="sstext">允许出库</div>
    </div>

    <div class="ssdivText">
        <div class="sstext">入库来料RFID:</div>
        <span class="sstext sstext_color" v-if="context != null">
            {{ context?.plcInfo?.Gateways?.EC010_A库口?.EntryRFID }}
        </span>
    </div>
    <div class="ssdivText">
        <div class="sstext">请求出库RFID:</div>
        <span class="sstext sstext_color" v-if="context != null">
            {{ context?.mstInfo?.Gateways?.EC010_A库口?.RequestTaskRFID }}
        </span>
    </div>
    <div class="ssdiv">
        <div class="sstext">出库校验结果:</div>
        <span class="sstext sstext_color" v-if="context != null">
            {{requestTaskResultOptions.find(u => u.key ==
                context?.plcInfo?.Gateways?.EC010_A库口?.RequestTaskResult)?.display_name}}
        </span>
    </div>
</div>
<!-- EC010_B库口 -->
<div class="text" type="flex" style="height: 1.5rem">
    <div class="ssdivText" style="font-weight: bold;">
        <div class="sstext">EC010_B库口</div>
    </div>
    <div class="ssdiv">
        <div v-if="context?.plcInfo?.Gateways?.EC010_B库口?.StandByReq" class="GreenBall tc"></div>
        <div v-else class="GreyBall tc"></div>
        <div class="sstext">入库就位请求</div>
    </div>
    <div class="ssdiv">
        <div v-if="context?.mstInfo?.Gateways?.EC010_B库口?.StandByAck" class="GreenBall tc"></div>
        <div v-else class="GreyBall tc"></div>
        <div class="sstext">入库就位确认</div>
    </div>
    <div class="ssdiv">
        <div v-if="context?.plcInfo?.Gateways?.EC010_B库口?.OutStatus" class="GreenBall tc"></div>
        <div v-else class="GreyBall tc"></div>
        <div class="sstext">允许出库</div>
    </div>

    <div class="ssdivText">
        <div class="sstext">入库来料RFID:</div>
        <span class="sstext sstext_color" v-if="context">
            {{ context.plcInfo?.Gateways?.EC010_B库口?.EntryRFID }}
        </span>
    </div>
    <div class="ssdivText">
        <div class="sstext">请求出库RFID:</div>
        <span class="sstext sstext_color" v-if="context!=null">
            {{ context?.mstInfo?.Gateways?.EC010_B库口?.RequestTaskRFID }}
        </span>
    </div>
    <div class="ssdiv">
        <div class="sstext">出库校验结果:</div>
        <span class="sstext sstext_color" v-if="context">
            {{
                requestTaskResultOptions.find(
                    u => u.key === context.plcInfo?.Gateways?.EC010_B库口?.RequestTaskResult
                )?.display_name
            }}
        </span>
    </div>
</div>



</template>

<script setup>
import { ref, defineProps, reactive, onUpdated, onBeforeUpdate, watch, onMounted } from 'vue'

const context = reactive({
    plcInfo: {}, mstInfo: {}, mstMsg: {}, CreatedAt: ""
})
const context_temp = defineProps({
    PlcInfo: Object,
    default: {},

    MstInfo: Object,
    default: {},
    MstMsg: Object,
    default: {},
    CreatedAt: String,
    default: ""
})

onUpdated(() => {

    context.plcInfo = context_temp.PlcInfo
    context.mstInfo = context_temp.MstInfo
    context.mstMsg = context_temp.MstMsg
    context.CreatedAt = context_temp.CreatedAt
})


const abc = ref(false)

const taskTypeOptions = reactive([{
    key: 0,
    display_name: "无工作命令"
}, {
    key: 1,
    display_name: "入库作业"
}, {
    key: 2,
    display_name: "出库作业"
}, {
    key: 3,
    display_name: "直接出库"
}, {
    key: 4,
    display_name: "移库"
}, {
    key: 5,
    display_name: "更换仓位"
}, {
    key: 7,
    display_name: "盘点"
}, {
    key: 8,
    display_name: "回原点"
}])

const storkerStatusOptions = reactive([{
    key: 0,
    display_name: "关"
}, {
    key: 1,
    display_name: "维修"
}, {
    key: 2,
    display_name: "手动"
}, {
    key: 3,
    display_name: "半自动"
}, {
    key: 4,
    display_name: "自动"
}, {
    key: 5,
    display_name: "联机"
}])

const stockerTripOptions = reactive([{
    key: 0,
    display_name: "无"
}, {
    key: 1,
    display_name: "待机"
}, {
    key: 2,
    display_name: "运行"
}, {
    key: 3,
    display_name: "异常"
}])

const stockerCargoOptions = reactive([{
    key: 0,
    display_name: "无"
}, {
    key: 1,
    display_name: "无货"
}, {
    key: 2,
    display_name: "有货"
}])

const requestTaskResultOptions = reactive([{
    key: 0,
    display_name: "初始化"
}, {
    key: 1,
    display_name: "通过"
}, {
    key: 2,
    display_name: "不通过"
}])

const stockerActionOptions = reactive([{
    key: 0,
    display_name: "无"
}, {
    key: 1,
    display_name: "前进"
}, {
    key: 2,
    display_name: "后退"
}, {
    key: 3,
    display_name: "上升"
}, {
    key: 4,
    display_name: "下降"
}, {
    key: 5,
    display_name: "左放货"
}, {
    key: 6,
    display_name: "左取货"
}, {
    key: 7,
    display_name: "右放货"
}, {
    key: 8,
    display_name: "右取货"
}])
</script>

<style></style>
