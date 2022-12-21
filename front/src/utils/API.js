import axios from "axios";

const apiClient = axios.create({
  baseURL: "https://localhost:44349/",
  responseType: "json"
});

apiClient.interceptors.response.use(
  async (res) => {
    const accessToken = localStorage.getItem("accessToken")
    axios.defaults.headers['Authorization'] = 'Bearer ' + accessToken;
    return res;
  },
  async (error) => {
    const originalRequest = error.config;
    if (error.response.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      console.log("Interceptor error 401")
      const refreshToken = localStorage.getItem("refreshToken");
      return apiClient.post(`api/authentication/reauthenticate`, { refreshToken }, {
        headers: { "Accept": "application/json", }
      })
        .then(response => {
          localStorage.setItem("accessToken", response.data.accessToken);
          localStorage.setItem("refreshToken", response.data.refreshToken);
          localStorage.setItem("Role", response.data.role);
          const accessToken = 'Bearer ' + localStorage.getItem("accessToken")
          originalRequest.headers['Authorization'] = accessToken;
          console.log("Interceptor dealed with 401")
          return apiClient(originalRequest);
        });

    }
    return Promise.reject(error)
  }
);

apiClient.interceptors.request.use(req => {
  apiClient.defaults.headers['Authorization'] = 'Bearer ' + localStorage.getItem("accessToken");
  return req;
});

export { apiClient };
