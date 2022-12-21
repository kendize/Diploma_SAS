import { createStore, combineReducers, applyMiddleware, compose } from 'redux';
import { DashboardReducer } from './reducers/DashboardReducer';
import { AuthenticationReducer } from './reducers/AuthenticationReducer';
import thunk from 'redux-thunk'
import { UserCourseReducer } from './reducers/UserCourseReducer';

const rootReducer = combineReducers({
    dashboard: DashboardReducer,
    authentication: AuthenticationReducer,
    userCourse: UserCourseReducer
})

const store = createStore(rootReducer, {}, compose(
    applyMiddleware(thunk),
    window.devToolsExtension ? window.devToolsExtension() : f => f
))

export default store;