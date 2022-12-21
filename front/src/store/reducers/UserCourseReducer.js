import { GET_USER_SUBSCRIPTIONS, LOGOUT } from "../actions";

const initialState = {
    userCourse: []
};

export const UserCourseReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_USER_SUBSCRIPTIONS: {
            return { ...state, userCourse: action.payload.userCourses }
        }
        case LOGOUT: {
            return { ...initialState }
        }
        default: {
            return state
        }
    }
}