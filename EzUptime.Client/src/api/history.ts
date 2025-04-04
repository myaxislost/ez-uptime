import type { AxiosInstance } from 'axios';
import { useAxiosInstance } from './useApi';
import type { MonitoringStepDto } from './monitoringStep';
import type { ConfigDto } from './config';

export interface MonitoringHistoryDto {
  created: string;
  results: MonitoringStepDto[];
  config: ConfigDto;
  numErrors: number;
  avgPing: number;
}

export class UptimeHistory {
  axios: AxiosInstance;

  constructor() {
    this.axios = useAxiosInstance();
  }

  fetchHistory(): Promise<{ [key: string]: MonitoringHistoryDto[] }> {
    return this.axios.get('/uptime').then((res) => res.data);
  }
}
