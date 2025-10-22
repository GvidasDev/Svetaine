import apiClient from "./apiClient";

const eventApi = {
  getAll: () => apiClient.get("/events"),
  getById: (id) => apiClient.get(`/events/${id}`),
  create: (event) => apiClient.post("/events", event),
  update: (id, event) => apiClient.put(`/events/${id}`, event),
  delete: (id) => apiClient.delete(`/events/${id}`),
};

export default eventApi;