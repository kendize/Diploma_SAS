import { apiClient } from "../../utils/API";
import { GET_USER_SUBSCRIPTIONS } from "../actions";

export const get_user_subscriptions = () => {
    return async (dispatch) => {
        await apiClient.get('/api/subscription/GetUserSubscriptions',
            {
                headers: {
                    "Accept": "application/json",
                    'Authorization': 'Bearer ' + localStorage.getItem("accessToken")
                },
            }
        )
            .then(
                ({ data }) => {
                    dispatch({
                        type: GET_USER_SUBSCRIPTIONS,
                        payload: data
                    })
                }
            )
            .catch(
                error => console.log("action creator error: " + error)
            )
    }
}