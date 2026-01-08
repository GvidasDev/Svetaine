import axios from "axios";

export const API_BASE_URL = "https://localhost:7192/api";
export const API_ORIGIN = API_BASE_URL.replace("/api", "");

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

export default apiClient;
