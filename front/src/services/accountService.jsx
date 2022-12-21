import { apiClient } from "../utils/API";

const accountService = {
    handleRegistration: async (registrationData) => {
        return apiClient.post("/api/admin", registrationData, {
            "Content-Type": "application/json"
        }); 
    },

    handleEmailConfirmation: (userid, code) => {
        return apiClient.get(`/api/authentication/confirmemail?${userid}&${code}`)
    }

    
}

export default accountService