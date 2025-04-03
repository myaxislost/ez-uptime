<script setup lang="ts">
import { useUptimeStore } from '@/stores/uptime';
import { onMounted, onUnmounted } from 'vue';
import UptimeBlock from '@/components/UptimeBlock.vue';
const { history, fetchHistory, stopFetching } = useUptimeStore();

onMounted(() => {
  fetchHistory();
});

onUnmounted(() => {
  stopFetching();
});
</script>

<template>
  <div class="view uptime-view">
    <div class="uptime-group" v-for="(results, group) in history.results" :key="group">
      <span style="grid-column: 1/-1">{{ group }}</span>
      <UptimeBlock v-for="c in results" :key="c.config.label" :history="c" />
    </div>
  </div>
</template>

<style>
.uptime-view {
  display: flex;
  flex-direction: column;
  gap: 15px;
}
.uptime-group {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 10px;
}

@media (max-width: 768px) {
  .uptime-group {
    grid-template-columns: 1fr;
  }
}

/* Tablets (Portrait Mode) */
@media (min-width: 769px) and (max-width: 1024px) {
  .uptime-group {
    grid-template-columns: repeat(3, 1fr);
  }
}
</style>
