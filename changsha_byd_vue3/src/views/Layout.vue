<template>
  <el-header>
    长沙比亚迪
  </el-header>
  <el-container class="layout-container-demo" style="height: 100vh">
    <!-- 侧边栏 -->
    <el-aside width="auto">

      <el-menu 
        default-active="/"
        class="el-menu-vertical-demo" 
        :collapse="isCollapse"
        @open="handleOpen"
        @close="handleClose"
        :router="true"
      >
        <!-- 菜单项1 -->
        <el-menu-item index="/">
          <el-icon><icon-menu /></el-icon>
          <span>首页</span>
        </el-menu-item>

        <!-- 菜单项2 -->
        <el-sub-menu index="2">
          <template #title>
            <el-icon>
              <location />
            </el-icon>
            <span>生产管理</span>
          </template>
          <el-menu-item-group>
            <el-menu-item index="/wms/task">作业管理</el-menu-item>
            <el-menu-item index="/wms/createTask">手动出入库</el-menu-item>
            <el-menu-item index="/wms/kucun">库存查询</el-menu-item>
            <el-menu-item index="/wms/taskRecord">出入库记录</el-menu-item>
            <el-menu-item index="/wms/enterAndOutRuleSetting">出入规则设置</el-menu-item>
          </el-menu-item-group>
        </el-sub-menu>

        <!-- 菜单项3 -->
        <el-sub-menu index="3">
          <template #title>
            <el-icon>
              <location />
            </el-icon>
            <span>设备管理</span>
          </template>
          <el-menu-item-group>
            <el-menu-item index="/wms/carType">车型管理</el-menu-item>
          </el-menu-item-group>
        </el-sub-menu>
      </el-menu>
    </el-aside>

    <!-- Main -->
    <el-container>
      <el-main>
        <!--展示内容 -->
        <router-view></router-view>
        <!-- <el-scrollbar>
          <el-table :data="tableData">
            <el-table-column prop="date" label="Date" width="140" />
            <el-table-column prop="name" label="Name" width="120" />
            <el-table-column prop="address" label="Address" />
          </el-table>
        </el-scrollbar> -->
      </el-main>
    </el-container>

  </el-container>
</template>

<script setup>
import { ref } from 'vue'
import {
  Document,
  Menu as IconMenu,
  Location,
  Setting,
} from '@element-plus/icons-vue'

//展开 关闭
const isCollapse = ref(false)
const item = {
  date: '2016-05-02',
  name: 'Tom',
  address: 'No. 189, Grove St, Los Angeles',
}
const tableData = ref(Array.from({ length: 20 }).fill(item))


const handleClose = () => {
  console.log('menu close')
}

const handleOpen = () => {
  console.log('menu open')
}
</script>

<style scoped>
::v-deep(.el-menu-item-group__title){
  padding: 0;
}




.layout-container-demo .el-header {
  position: relative;
  background-color: var(--el-color-primary-light-7);
  color: var(--el-text-color-primary);
}

.layout-container-demo .el-aside {
  color: var(--el-text-color-primary);
  background: var(--el-color-primary-light-8);
}

.layout-container-demo .el-menu {
  border-right: none;
}

.layout-container-demo .el-main {
  padding: 0;
}

.layout-container-demo .toolbar {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  height: 100%;
  right: 20px;
}
</style>