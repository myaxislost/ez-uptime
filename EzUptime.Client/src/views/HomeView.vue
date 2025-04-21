<script setup lang="ts">
import { useUptimeStore } from '@/stores/uptime';
import { onBeforeUnmount, onMounted } from 'vue';
import UptimeBlock from '@/components/UptimeBlock.vue';
import TagItem from '@/components/TagItem.vue';
const { history, startFetching, stopFetching } = useUptimeStore();

onMounted(() => {
  startFetching();
});
onBeforeUnmount(() => {
  stopFetching();
});

const isGroupOpened = (groupName: string | number) => {
  return history.groups[groupName];
};
const toggleGroup = (groupName: string | number) => {
  history.groups[groupName] = !history.groups[groupName];
};

const findNumProblems = (groupName: string | number) => {
  const group = history.results[groupName];
  let numProblems = 0;
  for (const groupName in group) {
    if (group[groupName].numErrors > 0) numProblems++;
  }
  return numProblems;
};
</script>

<template>
  <main>
    <group v-for="(results, group) in history.results" :key="group">
      <span class="header" @click="toggleGroup(group)">
        <fa-icon icon="chevron-down" v-if="isGroupOpened(group)" />
        <fa-icon icon="chevron-up" v-else />
        {{ group }}
        <TagItem v-if="findNumProblems(group)" color="var(--color-yellow)" style="gap: 10px">
          {{ findNumProblems(group) }}
          <fa-icon icon="warning" />
        </TagItem>
      </span>
      <template v-if="isGroupOpened(group)">
        <UptimeBlock v-for="c in results" :key="c.config.label" :history="c" />
      </template>
    </group>
  </main>
</template>

<style scoped>
group {
  display: grid;
  grid-template-columns: repeat(3, minmax(0, 1fr));
  gap: var(--gap);
  user-select: none;
}
group > .header {
  grid-column: 1/-1;
  display: flex;
  gap: var(--gap);
  align-items: center;
}

@media (max-width: 768px) {
  group {
    grid-template-columns: 1fr;
  }
}

/* Tablets (Portrait Mode) */
@media (min-width: 769px) and (max-width: 1024px) {
  group {
    grid-template-columns: repeat(3, minmax(0, 1fr));
  }
}
</style>
