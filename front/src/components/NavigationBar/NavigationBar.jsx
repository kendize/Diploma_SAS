import {
    BrowserRouter as Router,
    Switch,
    Link
} from "react-router-dom";
import React from 'react';
import Registration from '../Registration/Registration';
import UserDashboard from '../Dashboard/UserDashboard'
import CourseDashboard from '../Dashboard/CourseDashboard'
//import Home from '../../Home'
import Login from '../Login/Login'
import { useDispatch, useSelector } from 'react-redux'
import { Button, Menu, Row, Col, Space, Card } from 'antd';
import { logout } from "../../store/actionCreators/Authentication";
import PublicRoute from "../../utils/PublicRoute";
import PrivateRoute from "../../utils/PrivateRoute";
import { getFirstName, getLastName, isLogin } from "../../utils";
import Courses from "../Courses/Courses";
import EmailConfirmation from "../Profile/EmailConfirmation";
import Error_401 from "../Errors/401";
import Profile from "../Profile/Profile";
import Home from "../Home/Home";

const { SubMenu } = Menu;


const NavigationBar = () => {
    const dispatch = useDispatch();
    const state = useSelector(
        (state) => {
            return {
                authorized: state.authentication.authorized,
                isAdmin: state.authentication.isAdmin
            }
        }
    )

    const auth = state.authorized

    const isAdmin = state.isAdmin

    return (
        <div style={{ backgroundColor: '#fafafa' }}>
            <Router>
                <div style={{ backgroundColor: '#2C294B', padding: '7px' }}>

                    <Row align="middle">
                        <Col offset={1} span={6}>
                            <Menu
                                mode="horizontal"
                                style={{ backgroundColor: "#2C294B" }}
                                theme='dark'
                                selectedKeys={[]}
                            >

                                <Menu.Item key='home'>
                                    <Link to="/">Home</Link>
                                </Menu.Item>
                                {auth
                                    ?
                                    <>
                                        {isAdmin
                                            &&
                                            <>
                                                <SubMenu key="DashboardSubMenu" title="Dashboard" style={{ backgroundColor: "#2C294B" }}>
                                                    <Menu.Item>
                                                        <Link to="/UserDashboard">Users</Link>
                                                    </Menu.Item>
                                                    <Menu.Item>
                                                        <Link to="/CourseDashboard">Courses</Link>
                                                    </Menu.Item>
                                                    <Menu.Item>
                                                        <a href="https://localhost:44349/Hangfire" >Hangfire</a>
                                                    </Menu.Item>
                                                </SubMenu>
                                            </>
                                        }
                                        <Menu.Item key='courses'>
                                            <Link to="/Courses">Courses</Link>
                                        </Menu.Item>
                                    </>
                                    :
                                    <>
                                        <Menu.Item key='login'>
                                            <Link to="/Login">Authentication</Link>
                                        </Menu.Item>

                                        <Menu.Item key='registration'>
                                            <Link to="/Registration"> Registration</Link>
                                        </Menu.Item>
                                    </>
                                }
                            </Menu>
                        </Col>
                        <Col offset={13}>
                            {
                                isLogin() ?
                                    <Card bodyStyle={{ padding: "12px" }}
                                        style={{ borderRadius: "15px" }}
                                        hoverable>
                                        <Space align="baseline">
                                            <Link to="/Profile">{getFirstName() + "  " + getLastName() + "  "}</Link>
                                            <Button
                                                onClick={() => {
                                                    dispatch(logout())
                                                }}>Logout
                                            </Button>
                                        </Space>
                                    </Card>
                                    :
                                    <span></span>
                            }
                        </Col>
                    </Row>
                </div>
                <br />


                <Switch>
                    <PublicRoute exact path="/" component={Home} />
                    <PrivateRoute path="/UserDashboard" component={UserDashboard} />
                    <PrivateRoute path="/CourseDashboard" component={CourseDashboard} />
                    <PrivateRoute path="/Profile" component={Profile} />
                    <PublicRoute path="/Courses" component={Courses} />
                    <PublicRoute path="/Login" component={Login} />
                    <PublicRoute path="/Registration" component={Registration} />
                    <PublicRoute exact path="/EmailConfirmation/:userid&:code" component={EmailConfirmation} />
                    <PublicRoute path="/401" component={Error_401} />
                </Switch>
            </Router>
        </div>
    )
}

export default NavigationBar;