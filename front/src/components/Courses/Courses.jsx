import React, { useState, useEffect } from 'react';
import { Col, Row, Spin, Divider } from 'antd';
import { get_page_of_courses } from '../../store/actionCreators/Dashboard';
import { get_user_subscriptions } from '../../store/actionCreators/UserCourse';
import { useDispatch, useSelector } from 'react-redux';
import store from '../../store/store';
import CourseCard from './CourseCard';
import { SyncOutlined } from '@ant-design/icons';
import { Redirect } from 'react-router';

export default function Courses() {
    const dispatch = useDispatch();
    const antIcon = <SyncOutlined style={{ fontSize: 36 }} spin />;
    const courseList = useSelector((store) => store.dashboard.courseList);
    const isLoading = useSelector((store) => store.dashboard.coursesLoading);
    const [currentPage, setCurrentPage] = useState(1)
    const [numberOfCourses, setNumberOfCourses] = useState(1)
    const [orderColumnName, setOrderColumnName] = useState("courseName")
    const [orderBy, setOrderBy] = useState("ascend")
    const [pageSize, setPageSize] = useState(100)
    const [searchString, setSearchString] = useState("")
    const state = useSelector(
        (state) => {
            return {
                authorized: state.authentication.authorized,
            }
        }
    )

    const userCourse = useSelector(
        (state) => {
            return {
                userCourse: state.userCourse.userCourse
            }
        }
    )
    const auth = state.authorized

    useEffect(() => {

        dispatch(get_page_of_courses(currentPage, pageSize, orderColumnName, orderBy, searchString));
        dispatch(get_user_subscriptions());
    }, [])

    useEffect(() => {
        setNumberOfCourses(store.getState().dashboard.numberOfCourses)
        setCurrentPage(currentPage)
    }, [store.getState().dashboard.courseList, store.getState().dashboard.numberOfCourses, currentPage, numberOfCourses])

    return (
        <div>{auth ?
            <>
                <br />
                <Spin
                    indicator={antIcon}
                    size="large"
                    spinning={isLoading}>
                    <Row gutter={[0, 24]}>
                        {
                            courseList.map((element) => {
                                return (
                                    <Col span={6} align="center">

                                        <CourseCard
                                            element={element}
                                            loading={isLoading}
                                        />
                                    </Col>
                                )
                            })
                        }
                    </Row>
                </Spin> </> : <Redirect exact to="/" />}
            <Divider></Divider>
        </div>
    )
}