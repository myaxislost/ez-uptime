<script setup lang="ts"></script>

<template>
  <div class="ui-block">
    <slot></slot>
  </div>
</template>

<style>
.ui-block {
  background-color: var(--color-bg-dark);
  border-radius: 8px;
  padding: 10px;
}
</style>
