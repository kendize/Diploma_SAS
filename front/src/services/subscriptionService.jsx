import { apiClient } from "../utils/API";

const subscriptionService = {
    handleUnsubscribe: async (courseId) => {
        return apiClient.post("/api/subscription/unsubscribe",
        {
            CourseId: courseId
        },
        { "Content-Type": "application/json" });
    },
    handleSubscribe: async (courseId, studyDate) => {
        return apiClient.post("/api/subscription/subscribe",
        {
            headers: {
                "Accept": "application/json",
                'Authorization': 'Bearer ' + localStorage.getItem("accessToken")
            },

            CourseId: courseId,
            StudyDate: studyDate

        },
        //{ "Content-Type": "application/json" }
    );
    }

    
}

export default subscriptionService