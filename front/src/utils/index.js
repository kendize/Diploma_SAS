import jwtDecode from 'jwt-decode';

export const isLogin = () => {
    if (localStorage.getItem("accessToken")) {
        return true;
    }
    return false;
}

export const isAdmin = () => {
    if (localStorage.getItem("accessToken")) {
        if (jwtDecode(localStorage.getItem("accessToken"))['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] === 'admin') {
            return true
        }
    }
    return false
}

export const getFirstName = () => {
    if (localStorage.getItem("accessToken")) {
        return jwtDecode(localStorage.getItem("accessToken"))['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']
    }
    return ""
}

export const getLastName = () => {
    if (localStorage.getItem("accessToken")) {
        return jwtDecode(localStorage.getItem("accessToken"))['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname']
    }
    return ""
}