<script setup lang="ts">
import type { MonitoringHistoryDto } from '@/api/history';
import UiBlock from './UiBlock.vue';
import TagItem from './TagItem.vue';

interface IProp {
  history: MonitoringHistoryDto;
}
defineProps<IProp>();
</script>

<template>
  <UiBlock class="uptime-block">
    <div class="uptime-block-header">
      <span>
        {{ history.config.label }}
        <fa-icon v-if="history.numErrors > 0" icon="warning" style="color: var(--color-yellow)" />
      </span>
      <a
        :href="'http://' + history.config.address.replace('http://', '').replace('https://', '')"
        target="_blank"
      >
        {{ history.config.address }}
      </a>
      <TagItem>{{ Math.floor(history.avgPing * 100) / 100 }} ms</TagItem>
    </div>

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
.uptime-block-header {
  display: grid;
  grid-template-columns: minmax(max-content, 90px) 1fr min-content;
  gap: 10px;
  justify-content: space-between;
}
.uptime-block-header > a {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  color: var(--color-text-inactive);
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
