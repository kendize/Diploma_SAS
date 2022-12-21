import { GET_PAGE_OF_COURSES, GET_PAGE_OF_USERS, LOGOUT, RELOAD } from "../actions";

const initialState = {
    userList: [],
    courseList: [],
    numberOfUsers: 0,
    numberOfCourses: 0,
    usersLoading: true,
    coursesLoading: true
};

export const DashboardReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_PAGE_OF_USERS: {
            return {...state, userList: action.payload.users, numberOfUsers: action.payload.numberOfUsers, usersLoading: false};
        }
        case GET_PAGE_OF_COURSES: {
            return {...state, courseList: action.payload.courses, numberOfCourses: action.payload.numberOfCourses, coursesLoading: false};
        }
        case LOGOUT: {
            return {...initialState }
        }
        case RELOAD: {
            return{...state, usersLoading: true, coursesLoading: true};
        }
    default: {
        return state
    }
    }
}