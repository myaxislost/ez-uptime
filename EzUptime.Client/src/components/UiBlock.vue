<script setup lang="ts"></script>

<template>
  <div class="ui-block">
    <slot></slot>
  </div>
</template>

<style scoped>
.ui-block {
  background-color: var(--color-block);
  padding: 4px;
  border-radius: calc(var(--radius) * 1.5);
  border: 1px solid var(--color-border);
  box-shadow: var(--shadow);
}
</style>

<style>
.flex-block {
  display: flex;
  flex-direction: column;
  gap: var(--gap);
}
</style>
