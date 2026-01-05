import apiClient from "./apiClient";

const accountApi = {
  getMe: () => apiClient.get("/account/me"),
  updateMe: (data) => apiClient.put("/account/me", data),
};

export default accountApi;
