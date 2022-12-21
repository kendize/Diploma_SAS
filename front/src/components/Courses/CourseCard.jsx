import React, { useState } from 'react';
import { SyncOutlined } from '@ant-design/icons';
import { get_page_of_courses } from '../../store/actionCreators/Dashboard';
import { get_user_subscriptions } from '../../store/actionCreators/UserCourse';
import { useDispatch, useSelector } from 'react-redux';
import { Button, Popconfirm, Space, Image, Card, DatePicker, Spin, notification } from 'antd';
import moment from 'moment';
import subscriptionService from '../../services/subscriptionService';

const CourseCard = ({ element }) => {
    const antIcon = <SyncOutlined style={{ fontSize: 36 }} spin />;
    const [date, setDate] = useState("")
    const dispatch = useDispatch();
    const [currentPage, setCurrentPage] = useState(1)
    const [orderColumnName, setOrderColumnName] = useState("courseName")
    const [orderBy, setOrderBy] = useState("ascend")
    const [pageSize, setPageSize] = useState(100)
    const [searchString, setSearchString] = useState("")
    const [isLoading, setIsLoading] = useState(false)

    const userCourse = useSelector(
        (state) => {
            return {
                userCourse: state.userCourse.userCourse
            }
        }
    )

    const isSubscribed = (courseId) => {
        return [...userCourse.userCourse].some(
            element => element.courseId === courseId
        );
    }

    const subscriptionDate = (id) => {
        return [...userCourse.userCourse]
            .find(element => element.courseId === id).studyDate
    }
    const handleSubscribe = (courseId, studyDate) => {
        setIsLoading(true)
        subscriptionService.handleSubscribe(courseId, studyDate)
            .then(
                () => {
                    dispatch(
                        get_page_of_courses(currentPage, pageSize, orderColumnName, orderBy, searchString)
                    )

                    dispatch(
                        get_user_subscriptions()
                    )

                    notification.success(
                        {
                            message: "Success",
                            description: "Subscribed successfully!",
                            duration: 2,
                            placement: 'bottomRight'
                        }
                    )
                }
            )
            .finally(
                () => {
                    setIsLoading(false)
                }
            )
            .catch(
                (error) => {
                    console.log(error)
                    setIsLoading(false)
                    notification.error(
                        {
                            message: "Error",
                            description: "Error occured, user was not subscribed",
                            duration: 2,
                            placement: 'bottomRight'
                        }
                    )
                }
            )
    }

    const handleUnSubscribe = (courseId) => {
        setIsLoading(true)
        subscriptionService.handleUnsubscribe(courseId)
            .then(
                () => {
                    dispatch(
                        get_page_of_courses(currentPage, pageSize, orderColumnName, orderBy, searchString)
                    )
                    dispatch(
                        get_user_subscriptions()
                    )
                    notification.success(
                        {
                            message: "Success",
                            description: "Unsubscribed successfully!",
                            duration: 2,
                            placement: 'bottomRight'
                        }
                    )
                }
            )
            .finally(
                () => {
                    setIsLoading(false)
                }
            )
            .catch(
                (error) => {
                    console.log(error)
                    setIsLoading(false)
                    notification.error(
                        {
                            message: "Error",
                            description: "Error occured, user was not unsubscribed",
                            duration: 2,
                            placement: 'bottomRight'
                        }
                    )
                }
            )
    }

    return (
        <Spin spinning={isLoading}>
            <Card
                key={element.id}
                hoverable
                title={element.courseName}
                style={{
                    width: '90%',
                    height: 480,
                    borderRadius: "25px"
                }
                }
                cover={

                    <Image
                        alt={element.id}
                        src={element.courseImgUrl}
                        width={"80%"}
                        preview={false}
                        placeholder={
                            <Spin indicator={antIcon}
                                size="large"></Spin>
                        }
                    />
                }
            >
                <div>
                    <Space direction="vertical">
                        {element.courseDescription}
                        {isSubscribed(element.id) ?
                            <>

                                <h4>Study Date: {subscriptionDate(element.id).split(' ')[0]}</h4>


                                <Popconfirm
                                    title="Unsubscribe from course?"
                                    onConfirm={() => handleUnSubscribe(element.id)}
                                    okText="Yes"
                                    cancelText="No">
                                    <Button
                                        danger
                                        size="middle"
                                    >
                                        Unsubscribe
                                    </Button>
                                </Popconfirm>
                            </>
                            :
                            <>
                                <DatePicker
                                    onChange={(date, dateString) => { setDate(dateString) }}
                                />
                                <Button
                                    size="middle"
                                    type="primary"
                                    onClick={() => handleSubscribe(element.id, moment(date).format())}
                                >
                                    Subscribe
                                </Button>
                            </>
                        }

                    </Space>
                </div>
            </Card>
        </Spin>
    )
}

export default CourseCard