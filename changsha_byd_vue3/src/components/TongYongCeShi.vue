<template>
    {{ context_temp.PlcInfo.heartBeatAck}}
</template>

<script setup>

import { defineProps, onUpdated, reactive,ref} from 'vue'

// const context = ref({
// PlcInfo:{}, MstInfo:{}, MstMsg:{}, CreatedAt:{}
// })
const context = reactive({
    PlcInfo:{}, MstInfo:{}, MstMsg:{}, CreatedAt:""
})

// //todo
const context_temp = defineProps({
    PlcInfo: Object, 
    MstInfo: Object,
    MstMsg: Object, 
    CreatedAt: String
})

onUpdated(() => {
    context.PlcInfo=context_temp.PlcInfo
    context.MstInfo=context_temp.MstInfo
    context.MstMsg=context_temp.MstMsg
    context.CreatedAt=context_temp.CreatedAt
})
</script>

<style scoped>
</style>