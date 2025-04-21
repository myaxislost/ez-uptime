<script setup lang="ts">
interface IProps {
  color?: string;
}
defineProps<IProps>();
</script>

<template>
  <div :style="{ backgroundColor: color ?? 'var(--color-bg)' }">
    <slot></slot>
  </div>
</template>

<style scoped>
div {
  display: flex;
  align-items: center;
  font-size: 0.8rem;
  background-color: var(--color-bg);
  padding: 0px 5px;
  border-radius: var(--radius);
  user-select: none;
  opacity: 0.8;
  white-space: nowrap;
}
</style>
