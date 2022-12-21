import { Card, Row, Col, Divider, Spin, Descriptions, Image } from "antd"
import { SyncOutlined } from '@ant-design/icons';
import { useEffect } from "react";
import { useSelector } from "react-redux";
import { useDispatch } from "react-redux"
import { Redirect } from "react-router";
import { get_page_of_courses } from "../../store/actionCreators/Dashboard";
import { get_user_subscriptions } from "../../store/actionCreators/UserCourse";
import CourseCard from "../Courses/CourseCard";
import { MehOutlined } from '@ant-design/icons';
import { getFirstName, getLastName } from "../../utils";
const Profile = () => {
    const antIcon = <SyncOutlined style={{ fontSize: 36 }} spin />;
    const dispatch = useDispatch();

    const state = useSelector(
        (state) => {
            return {
                authorized: state.authentication.authorized,
            }
        }
    )
    const auth = state.authorized
    const courseList = useSelector((store) => store.dashboard.courseList);
    const isLoading = useSelector((store) => store.dashboard.coursesLoading);

    const userCourse = useSelector(
        (state) => {
            return {
                userCourse: state.userCourse.userCourse
            }
        }
    )

    const isAnySubscription = () => {
        if ([...userCourse.userCourse].length === 0) {
            return false
        }
        return true
    }

    const isSubscribed = (courseId) => {
        return [...userCourse.userCourse].some(
            element => element.courseId === courseId
        );
    }

    useEffect(() => {

        dispatch(get_page_of_courses(1, 100, "courseName", "ascend", ""));
        dispatch(get_user_subscriptions());
    }, [])
    return (
        <>
            <Row>
                <Col span={5} offset={1}>
                    <Card hoverable style={{ borderRadius: "25px" }}>
                        <Image
                            src='https://t3.ftcdn.net/jpg/03/91/19/22/360_F_391192211_2w5pQpFV1aozYQhcIw3FqA35vuTxJKrB.jpg'
                            preview={false}
                            placeholder={
                                <Spin indicator={antIcon}
                                    size="large"></Spin>}></Image>
                    </Card>
                </Col>
                <Col span={16} offset={1}>
                    <Card hoverable style={{ borderRadius: "25px" }} title="User Information">
                        <Descriptions bordered labelStyle={{ width: '20%' }}>
                            <Descriptions.Item label="First Name " span={6}>
                                {getFirstName()}
                            </Descriptions.Item>
                            <Descriptions.Item label="Last Name " span={6}>
                                {getLastName()}
                            </Descriptions.Item>
                            <Descriptions.Item label="Age " span={6}>
                                {localStorage.getItem("age") !== 0
                                    ?
                                    localStorage.getItem("age")
                                    :
                                    <span>Не вказано</span>}
                            </Descriptions.Item>
                            <Descriptions.Item label="Email"
                                span={6}>
                                {
                                    localStorage.getItem("email").length !== 0 && localStorage.getItem("email") !== 'null'
                                        ?
                                        localStorage.getItem("email")
                                        :
                                        <span>Не вказано</span>
                                }
                            </Descriptions.Item>

                        </Descriptions>
                    </Card>
                </Col>
            </Row>

            <div>{auth ?
                <>
                    <br />
                    <Spin
                        size="large"
                        spinning={isLoading}>
                        <Row gutter={[0, 8]}
                        >

                            {
                                isAnySubscription() ?
                                    courseList.map((element) => {
                                        return (
                                            isSubscribed(element.id) ?
                                                <Col span={6} align="center">

                                                    <CourseCard
                                                        element={element}
                                                        loading={isLoading}
                                                    />

                                                </Col>
                                                : <span></span>

                                        )
                                    })
                                    : <Col span={6} offset={8} align="center">
                                        <span align="center">
                                            <Card style={{ width: "500px" }}>
                                                <MehOutlined style={{ fontSize: 56 }} />
                                                <br></br>
                                                Sorry, but your'e not subscribed on any course
                                            </Card>
                                        </span>
                                    </Col>
                            }
                        </Row>
                    </Spin>
                </>
                :
                <Redirect exact to="/" />}
                <Divider></Divider>
            </div>
        </>
    )
}
export default Profile