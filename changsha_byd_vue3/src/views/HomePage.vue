<template>
    <TongYongQH v-bind="context_temp"></TongYongQH>
	<TongYongQHWrite></TongYongQHWrite>
</template>

<script setup>
import TongYongQH from '@/components/TongYongQH.vue'
import { onMounted, ref} from 'vue';
import * as signalrs from '@/utils/signalr'
import TongYongQHWrite from '@/components/TongYongQHWrite.vue';

const context_temp = ref({})


onMounted(() => {
    signalrs.startNewConnection('duiduoji')
    window.RefreshPlcMsg = RefreshPlcMsg
    window.RefreshMsg = RefreshMsg
})

const RefreshPlcMsg = (data, no) => {
    console.log('Received data:', data, 'no:', no);
    if (no == 1) {
		
    } else if (no == 2) {
        context_temp.value = data;
    }
}


const RefreshMsg = (data) => {
    if (data != null) {
        data.createTime = formatTime('hh:mm:ss')
        msg.value.unshift(data)
    }

}
</script>

<style>
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