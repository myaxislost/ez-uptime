import type { MonitoringHistoryDto } from '@/api/history';
import { useApi } from '@/api/useApi';
import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useUptimeStore = defineStore('uptime', () => {
  const { api } = useApi();
  const history = ref({
    results: {} as { [key: string]: MonitoringHistoryDto[] },
    groups: {} as { [key: string]: boolean },
    interval: 0,
  });

  const fetchHistory = async () => {
    history.value.results = await api.history.fetchHistory();

    for (const g in history.value.results) {
      if (history.value.groups[g] == undefined) {
        history.value.groups[g] = true;
      }
    }
  };

  const startFetching = async () => {
    fetchHistory();
    if (history.value.interval > 0) return;

    history.value.interval = setInterval(async () => {
      fetchHistory();
    }, 2000);
  };

  const stopFetching = () => {
    clearInterval(history.value.interval);
  };

  return { history, fetchHistory, startFetching, stopFetching };
});
