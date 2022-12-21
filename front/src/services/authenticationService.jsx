import { apiClient } from "../utils/API";


const authenticationService = {

  handleFacebookLogin: async (accessToken) => {
    return apiClient.get(`/api/authentication/authenticatefacebook`,
      { params: { accessToken: accessToken } });
  },
  
  handleLogin: async (Email, Password) => {
    return apiClient.post("/api/authentication/authenticate", { Email, Password }, {
      "Content-Type": "application/json"
    })
  }
}

export default authenticationService