import apiClient from "./apiClient";

const uploadApi = {
  uploadImage: (file) => {
    const fd = new FormData();
    fd.append("file", file);

    return apiClient.post("/upload/image", fd, {
    headers: { "Content-Type": "multipart/form-data" }
    });
  }
};

export default uploadApi;
