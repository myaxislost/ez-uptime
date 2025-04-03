<script setup lang="ts">
import type { MonitoringHistoryDto } from '@/api/history';
import UiBlock from './UiBlock.vue';
import { computed } from 'vue';
import TagItem from './TagItem.vue';

interface IProp {
  history: MonitoringHistoryDto;
}
const props = defineProps<IProp>();

const avgPing = computed(() => {
  const pings = props.history.results.filter((x) => x.success).map((x) => x.ping);
  if (pings.length == 0) return 0;

  const average = pings.reduce((a, b) => a + b, 0) / pings.length;
  return Math.floor(average * 100) / 100;
});
</script>

<template>
  <UiBlock class="uptime-block">
    <div style="display: flex; justify-content: space-between">
      <span>{{ history.config.label }}</span>
      <TagItem>{{ avgPing }} ms</TagItem>
    </div>
    <a v-if="history.config.type == 'HttpGet'" :href="history.config.address" target="_blank">{{
      history.config.address
    }}</a>
    <a
      v-else-if="history.config.type == 'Ping'"
      :href="'http://' + history.config.address"
      target="_blank"
      >{{ history.config.address }}</a
    >
    <div
      class="uptime-bar"
      :style="{
        gridTemplateColumns: `repeat(${history.config.resultsCap}, 1fr)`,
      }"
    >
      <div
        class="uptime-step"
        :class="{ 'uptime-step-failure': !r.success }"
        v-for="r in history.results"
        :key="r.timestamp + history.config.label"
      ></div>
    </div>
  </UiBlock>
</template>

<style>
.uptime-block {
  display: flex;
  flex-direction: column;
  justify-content: center;
  gap: 5px;
}
.uptime-bar {
  display: grid;
  gap: 3px;
}
.uptime-step {
  background-color: var(--color-accent);
  min-width: 3px;
  min-height: 10px;
}

.uptime-step-failure {
  background-color: var(--color-red) !important;
}
</style>
