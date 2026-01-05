import apiClient from "./apiClient";

const base = "/task";

const taskApi = {
  getAll: () => apiClient.get(base),
  create: (data) => apiClient.post(base, data),
  update: (id, data) => apiClient.put(`${base}/${id}`, data),
  delete: (id) => apiClient.delete(`${base}/${id}`)
};

export default taskApi;