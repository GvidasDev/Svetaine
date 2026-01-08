import apiClient from "./apiClient";

const invitationApi = {
  lookup: (email) => apiClient.get(`/invitation/lookup?email=${encodeURIComponent(email)}`),
  getInvited: (eventId) => apiClient.get(`/invitation/${eventId}`),
  add: (eventId, email) => apiClient.post(`/invitation/${eventId}`, { email }),
  remove: (eventId, userId) => apiClient.delete(`/invitation/${eventId}/${userId}`)
};

export default invitationApi;
