import apiClient from "./apiClient";

const expenseApi = {
  getAll: (eventId) => apiClient.get(`/events/${eventId}/expenses`),
  add: (eventId, data) => apiClient.post(`/events/${eventId}/expenses`, data),
  getBalances: (eventId) => apiClient.get(`/events/${eventId}/expenses/balances`)
};

export default expenseApi;
