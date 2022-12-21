import {LOGOUT} from '../actions';

export const logout = () => {
  localStorage.removeItem("accessToken");
  localStorage.removeItem("refreshToken");
  localStorage.removeItem("age")
  localStorage.removeItem("email")
  return {
    type: LOGOUT,
    authorized: false
  }
}
