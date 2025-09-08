<template>
  <div>这是changsha_byd---vue3</div>
</template>

<script setup>
import service from '@/utils/http';
import { ref, computed, onMounted } from 'vue'

onMounted(() => {
  service.get('/test').then(res => {
    console.log(res);
  })
})



</script>

<style scoped></style>
