import apiClient from "./apiClient";

const authApi = {
  register: (payload) => apiClient.post("/auth/register", payload),
  login: (payload) => apiClient.post("/auth/login", payload),
};

export default authApi;
