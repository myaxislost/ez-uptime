import type { MonitoringHistoryDto } from '@/api/history';
import { useApi } from '@/api/useApi';
import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useUptimeStore = defineStore('uptime', () => {
  const { api } = useApi();
  const history = ref({
    results: {} as { [key: string]: MonitoringHistoryDto[] },
    interval: 0,
  });

  const fetchHistory = async () => {
    history.value.results = await api.history.fetchHistory();
    if (history.value.interval > 0) return;

    history.value.interval = setInterval(async () => {
      history.value.results = await api.history.fetchHistory();
    }, 2000);
  };

  const stopFetching = () => {
    clearInterval(history.value.interval);
  };

  return { history, fetchHistory, stopFetching };
});
