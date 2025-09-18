<template>
    <!-- 手动出入库记录 -->
    <el-tabs type="border-card" class="demo-tabs">
        <el-tab-pane label="手动入库">
            <EnterLocation></EnterLocation>
        </el-tab-pane>
        <el-tab-pane label="手动出库">
            <OutLocation></OutLocation>
        </el-tab-pane>
    </el-tabs>
</template>

<script setup>
import EnterLocation from '@/components/EnterAndOutRecord/EnterLocation.vue';
import OutLocation from '@/components/EnterAndOutRecord/OutLocation.vue';

</script>

<style scoped>
.demo-tabs > .el-tabs__content {
  padding: 32px;
  color: #6b778c;
  font-size: 32px;
  font-weight: 600;
}
.demo-tabs .custom-tabs-label .el-icon {
  vertical-align: middle;
}
.demo-tabs .custom-tabs-label span {
  vertical-align: middle;
  margin-left: 4px;
}
</style>