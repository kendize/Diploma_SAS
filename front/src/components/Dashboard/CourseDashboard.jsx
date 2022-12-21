import React, { useState, useEffect } from 'react';
import { SyncOutlined } from '@ant-design/icons';
import { Form, Button, Pagination, Table, Popconfirm, Input, Row, Col, Space, Modal, Typography, Image, Spin, notification } from 'antd';
import { get_page_of_courses } from '../../store/actionCreators/Dashboard';
import { useDispatch, useSelector } from 'react-redux';
import store from '../../store/store';
import { Redirect } from 'react-router';
import { RELOAD } from '../../store/actions';
import dashboardService from '../../services/dashboardService';
const { Search } = Input;
const { TextArea } = Input;
const { Text } = Typography;

export default function CourseDashboard() {
    const antIcon = <SyncOutlined style={{ fontSize: 36 }} spin />;
    const dispatch = useDispatch();
    const courseList = useSelector((store) => store.dashboard.courseList);
    const [currentPage, setCurrentPage] = useState(1)
    const [numberOfCourses, setNumberOfCourses] = useState(1)
    const [orderColumnName, setOrderColumnName] = useState("Id")
    const [orderBy, setOrderBy] = useState("ascend")
    const [pageSize, setPageSize] = useState(5)
    const [searchString, setSearchString] = useState("")

    const [editCourseVisible, setEditCourseVisible] = useState(false);
    const [createCourseVisible, setCreateCourseVisible] = useState(false);

    const [courseId, setCourseId] = useState("");
    const [courseName, setCourseName] = useState("")
    const [courseDescription, setCourseDescription] = useState("")
    const [courseImgUrl, setCourseImgUrl] = useState("")

    const [expandedKey, setExpandedKey] = useState([])
    const isLoading = useSelector((store) => store.dashboard.coursesLoading);
    const handleExpand = (expanded, record) => {
        if (expanded) {
            setExpandedKey([record.id])
        }
        else {
            setExpandedKey("");
        }
    }

    const showEditCourseModal = (record) => {
        setCourseId(record.id);
        setCourseName(record.courseName);
        setCourseDescription(record.courseDescription);
        setCourseImgUrl(record.courseImgUrl);
        setEditCourseVisible(true);
    };

    const showCreateCourseModal = () => {
        setCourseId("");
        setCourseName("");
        setCourseDescription("");
        setCourseImgUrl("");
        setCreateCourseVisible(true);
    };

    const hideEditCourseModal = () => {
        setEditCourseVisible(false);
    };

    const hideCreateCourseModal = () => {
        setCreateCourseVisible(false);
    };

    const handleEditCourseModal = () => {
        dispatch(
            {
                type: RELOAD
            }
        )
        dashboardService.handleCourseEdit(courseId, courseName, courseDescription, courseImgUrl)
            .then((response) => {
                console.log(response);
                setEditCourseVisible(false);

                dispatch(get_page_of_courses(currentPage, pageSize, orderColumnName, orderBy, searchString));
                notification.success(
                    {
                        message: "Success",
                        description: "Course Edit Success!",
                        duration: 2,
                        placement: 'bottomRight'
                    }
                )
            })
            .catch((error) => {
                console.log(error);
                notification.error(
                    {
                        message: "Error",
                        description: "Course Edit Error!",
                        duration: 2,
                        placement: 'bottomRight'
                    }
                )
            });

    }

    const handleCreateCourseModal = () => {
        dispatch(
            {
                type: RELOAD
            }
        )
        dashboardService.handleCourseCreate(courseName, courseDescription, courseImgUrl)
            .then(function (response) {
                console.log(response);
                setCreateCourseVisible(false);
                dispatch(get_page_of_courses(currentPage, pageSize, orderColumnName, orderBy, searchString));
                notification.success(
                    {
                        message: "Success",
                        description: "Course created successfully!",
                        duration: 2,
                        placement: 'bottomRight'
                    }
                )
            })
            .catch(function (error) {
                console.log(error);
                notification.error(
                    {
                        message: "Error",
                        description: "Course creation Error!",
                        duration: 2,
                        placement: 'bottomRight'
                    }
                )
            });

    }

    const handleDelete = (id) => {
        dispatch(
            {
                type: RELOAD
            }
        )
        dashboardService.handleCourseDelete(id)
            .then(
                () => {
                    console.log(id);
                    setCurrentPage(1);
                    dispatch(get_page_of_courses(1, pageSize, orderColumnName, orderBy, searchString));
                    setNumberOfCourses(store.getState().dashboard.numberOfCourses)
                    notification.success(
                        {
                            message: "Success",
                            description: "Course Deleted Successfully!",
                            duration: 2,
                            placement: 'bottomRight'
                        }
                    )

                })
            .catch(
                (error) => {
                    console.log(error)
                    notification.error(
                        {
                            message: "Error",
                            description: "Course Delete Error!",
                            duration: 2,
                            placement: 'bottomRight'
                        }
                    )
                }
            )
    }

    const handleSorting = (pagination, filters, sorter) => {
        dispatch(
            {
                type: RELOAD
            }
        )
        setOrderColumnName(sorter.field)
        setOrderBy(sorter.order)
        dispatch(get_page_of_courses(currentPage, pageSize, sorter.field, sorter.order, searchString))
        console.log(sorter.field)
        console.log(sorter.order)
        console.log(pagination)
    }

    const handleChangeOfPage = (pageNumber, ColumnName, OrderBy, SearchString) => {
        dispatch(
            {
                type: RELOAD
            }
        )
        setOrderBy(OrderBy);
        setCurrentPage(pageNumber);
        setOrderColumnName(ColumnName);
        setSearchString(SearchString);
        dispatch(get_page_of_courses(pageNumber, pageSize, ColumnName, OrderBy, SearchString));
    }

    useEffect(() => {
        dispatch(get_page_of_courses(currentPage, pageSize, orderColumnName, orderBy, searchString));
    }, [])

    useEffect(() => {
        setNumberOfCourses(store.getState().dashboard.numberOfCourses)
        setCurrentPage(currentPage)
    }, [store.getState().dashboard.courseList, store.getState().dashboard.numberOfCourses, currentPage, numberOfCourses])

    const dashboardColumns = [
        {
            title: "Course Name",
            dataIndex: 'courseName',
            key: 'courseName',
            sorter: true,
            sortDirections: ['ascend', 'descend', 'ascend'],
            width: "75%"
        },

        {
            title: 'Action',
            key: 'action',
            render: (record) => (
                <Space align="center">
                    <Button
                        size="middle"
                        type="primary"
                        onClick={() => showEditCourseModal(record)}
                    >
                        Edit course
                    </Button>

                    <Popconfirm
                        title="Delete this course?"
                        onConfirm={() => handleDelete(record.id)}
                        onCancel={(e) => console.log(e)}
                        okText="Yes"
                        cancelText="No">
                        <Button
                            danger
                            size="middle"
                        >
                            Delete
                        </Button>

                    </Popconfirm>
                </Space>
            ),
        },
    ];
    const state = useSelector(
        (state) => {
            return {
                authorized: state.authentication.authorized,
                isAdmin: state.authentication.isAdmin
            }
        }
    )
    const isAdmin = state.isAdmin
    return (
        <>
            {isAdmin
                ?
                <div>
                    <Modal
                        visible={editCourseVisible}
                        title="Edit course"
                        closable={false}
                        onCancel={hideEditCourseModal}
                        onOk={handleEditCourseModal}

                    >
                        <Form>
                            <Space direction="vertical" align="start">
                                <Space direction="vertical">
                                    <Text>Course Name</Text>
                                    <Form.Item>
                                        <Input
                                            type="text"
                                            value={courseName}
                                            placeholder="Course Name"
                                            onChange={event => setCourseName(event.target.value)}
                                            style={{ width: '400px' }}
                                        />
                                    </Form.Item>
                                </Space>
                                <Space direction="vertical">
                                    <Text>Course Description</Text>
                                    <Form.Item>
                                        <Input
                                            type="text"
                                            value={courseDescription}
                                            placeholder="Course Description"
                                            onChange={event => setCourseDescription(event.target.value)}
                                            style={{ width: '400px' }}
                                        />
                                    </Form.Item>
                                </Space>
                                <Space direction="vertical">
                                    <Form.Item>
                                        <Text>Course Image URL</Text>
                                        <TextArea
                                            autoSize={{ minRows: 2, maxRows: 6 }}
                                            type="text"
                                            value={courseImgUrl}
                                            placeholder="Course Image URL"
                                            onChange={event => setCourseImgUrl(event.target.value)}
                                            style={{ width: '400px' }}
                                        />
                                    </Form.Item>
                                </Space>
                            </Space>
                        </Form>
                    </Modal>

                    <Modal
                        visible={createCourseVisible}
                        title="Create course"
                        closable={false}
                        onOk={handleCreateCourseModal}
                        onCancel={hideCreateCourseModal}
                    >
                        <Form>
                            <Space direction="vertical" align="start">
                                <Space direction="vertical">
                                    <Text>Course Name</Text>
                                    <Form.Item>
                                        <Input
                                            type="text"
                                            value={courseName}
                                            placeholder="Course Name"
                                            onChange={event => setCourseName(event.target.value)}
                                            style={{ width: '400px' }}
                                        />
                                    </Form.Item>
                                </Space>
                                <Space direction="vertical">
                                    <Text>Course Description</Text>
                                    <Form.Item>
                                        <Input
                                            type="text"
                                            value={courseDescription}
                                            placeholder="Course Description"
                                            onChange={event => setCourseDescription(event.target.value)}
                                            style={{ width: '400px' }}
                                        />
                                    </Form.Item>
                                </Space>
                                <Space direction="vertical">
                                    <Form.Item>
                                        <Text>Course Image URL</Text>
                                        <TextArea
                                            autoSize={{ minRows: 2, maxRows: 6 }}
                                            type="text"
                                            value={courseImgUrl}
                                            placeholder="Course Image URL"
                                            onChange={event => setCourseImgUrl(event.target.value)}
                                            style={{ width: '400px' }}
                                        />
                                    </Form.Item>
                                </Space>
                            </Space>
                        </Form>
                    </Modal>
                    <Row>
                        <Col offset={1} span={22}>
                            <Search placeholder="Search by Name:" allowClear onSearch={(string) => {
                                handleChangeOfPage(currentPage, orderColumnName, orderBy, string)
                            }}
                                style={{ width: 200 }} /><br /><br />
                            <Spin
                                indicator={antIcon}
                                size="large"
                                spinning={isLoading}>
                                <Table
                                    dataSource={courseList}
                                    columns={dashboardColumns}
                                    pagination={false}
                                    onChange={handleSorting}
                                    onExpand={(expanded, record) => { handleExpand(expanded, record); }}
                                    rowKey="id"
                                    expandedRowKeys={expandedKey}
                                    expandable={
                                        {
                                            expandedRowRender: (record) =>
                                                <div>
                                                    <Image
                                                        width={300}
                                                        src={record.courseImgUrl}
                                                    />
                                                    <p>
                                                        <h4>
                                                            <b>Description: </b>
                                                            {record.courseDescription}
                                                        </h4>
                                                    </p>
                                                </div>
                                        }
                                    }

                                />
                            </Spin>
                            <br />
                            <Space align="end">

                                <Pagination сurrent={currentPage}
                                    pageSize={5}
                                    total={numberOfCourses}
                                    onChange={(page) => handleChangeOfPage(page, orderColumnName, orderBy, searchString)} />
                                <Button
                                    size="middle"
                                    type="primary"
                                    onClick={() => showCreateCourseModal()}
                                >
                                    Create course
                                </Button>
                            </Space>
                        </Col>
                    </Row>
                </div>
                :
                <Redirect exact to="/401" />
            }
        </>
    );

}
