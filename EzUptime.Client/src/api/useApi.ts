import axios, { type AxiosInstance } from 'axios';
import { UptimeHistory } from './history';

export class Api {
  axios: AxiosInstance;
  history: UptimeHistory;

  constructor() {
    this.axios = useAxiosInstance();
    this.history = new UptimeHistory();
  }
}

let api = null as Api | null;
export const useApi = () => {
  if (api == null) {
    api = new Api();
  }
  return { api };
};

let axiosInstance: AxiosInstance | null = null;

export function useAxiosInstance() {
  const baseUrl = `${window.location.protocol}//${window.location.host}/api`;

  if (axiosInstance == null) {
    axiosInstance = axios.create({
      baseURL: baseUrl,
      transitional: {
        silentJSONParsing: true,
      },
    });
  }

  const token = document.cookie.split('=')[1];
  if (token) axiosInstance.defaults.headers.common['Authorization'] = token.split(';')[0];

  return axiosInstance;
}
