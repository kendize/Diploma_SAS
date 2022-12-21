import { apiClient } from "../utils/API";

const dashboardService = {
    handleCourseDelete: (id) => {
        return apiClient.delete(`/api/course/${id}`);
    },

    handleCourseCreate: (courseName, courseDescription, courseImgUrl) => {
        return apiClient.post("/api/course", {
            coursename: courseName,
            courseDescription: courseDescription,
            courseImgUrl: courseImgUrl
        },
            {
                "Content-Type": "application/json"
            })
    },

    handleCourseEdit: (courseId, courseName, courseDescription, courseImgUrl) => {
        return apiClient.put("/api/course", { 
            id: courseId, 
            coursename: courseName, 
            courseDescription: courseDescription, 
            courseImgUrl: courseImgUrl 
        }, 
        {
            "Content-Type": "application/json"
        })
    },

    handleUserEdit: (userId, firstName, lastName, age, email) => {
        return apiClient.put("/api/admin", {
            id: userId,
            firstname: firstName,
            lastname: lastName,
            age: age,
            email: email
          }, {
            "Content-Type": "application/json"
          })
    }, 
    handleUserDelete: (id) => {
        return apiClient.delete(`/api/admin/${id}`,
      {
        headers: {
          "Accept": "application/json",
          'Authorization': 'Bearer ' + localStorage.getItem("accessToken")
        },
      })
    }
}

export default dashboardService