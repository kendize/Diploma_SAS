import { getFirstName, getLastName, isAdmin, isLogin } from "../../utils";
import { LOGIN, LOGOUT, RELOGIN } from "../actions";
const initialState = {
    accessToken: "",
    refreshToken: "",
    id: "",
    firstName: getFirstName(),
    lastName: getLastName(),
    isAdmin: isAdmin(),
    authorized: isLogin()
};

export const AuthenticationReducer = (state = initialState, action) => {
    switch (action.type) {
        case RELOGIN:
        case LOGIN: {
            const { accessToken, refreshToken, id, firstName, lastName } = action.payload;
            const authorized = action.authorized;
            const isAdmin = action.isAdmin;
            return { ...state, accessToken, refreshToken, id, firstName, lastName, isAdmin, authorized }
        }
        case LOGOUT: {
            const authorized = false;
            const isAdmin = false;
            return { ...initialState, authorized, isAdmin };
        }
        default:
            {
                return state;
            }
    }
}