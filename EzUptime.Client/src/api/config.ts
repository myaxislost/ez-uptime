export type MonitorType = 'HttpGet' | 'Ping';

export interface ConfigDto {
  label: string;
  type: MonitorType;
  address: string;
  period: number;
  resultsCap: number;
}
