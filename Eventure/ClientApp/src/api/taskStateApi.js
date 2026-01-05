import apiClient from "./apiClient";

const taskStateApi = {
  getAll: () => apiClient.get("/taskstate"),
  create: (name) => apiClient.post("/taskstate", name),
  delete: (id) => apiClient.delete(`/taskstate/${id}`),
};

export default taskStateApi;