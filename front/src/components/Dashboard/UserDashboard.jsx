import React, { useState, useEffect } from 'react';
import { SyncOutlined } from '@ant-design/icons';
import { Form, Button, Pagination, Table, Popconfirm, Input, Col, Space, Modal, Typography, Spin, Select, Divider, Row, notification } from 'antd';
import { get_page_of_users } from '../../store/actionCreators/Dashboard';
import { useDispatch, useSelector } from 'react-redux';
import store from '../../store/store';
import { Redirect } from 'react-router';
import { RELOAD } from '../../store/actions';
import dashboardService from '../../services/dashboardService';
const { Search } = Input;
const { Text } = Typography;
const { Option } = Select;

export default function UserDashboard() {
  const antIcon = <SyncOutlined style={{ fontSize: 36 }} spin />;
  const dispatch = useDispatch();
  const userList = useSelector((store) => store.dashboard.userList);
  const [currentPage, setCurrentPage] = useState(1)
  const [numberOfUsers, setNumberOfUsers] = useState(1)
  const [orderColumnName, setOrderColumnName] = useState("Id")
  const [orderBy, setOrderBy] = useState("ascend")
  const [pageSize, setPageSize] = useState(5)
  const [searchString, setSearchString] = useState("")
  const [searchColumn, setSearchColumn] = useState("FirstName")

  const [editUserVisible, setEditUserVisible] = useState(false);

  const [userId, setUserId] = useState("");
  const [Email, setEmail] = useState("")
  const [FirstName, setFirstName] = useState("")
  const [LastName, setLastName] = useState("")
  const [Age, setAge] = useState("")
  const isLoading = useSelector((store) => store.dashboard.usersLoading);
  const [expandedKey, setExpandedKey] = useState([])

  const changeSearchColumn = (value) => {
    setSearchColumn(value)
  }

  const handleExpand = (expanded, record) => {
    if (expanded) {
      setExpandedKey([record.id])
    }
    else {
      setExpandedKey("");
    }
  }

  const showEditUserModal = (record) => {
    setUserId(record.id);
    setEmail(record.email);
    setAge(record.age);
    setFirstName(record.firstName);
    setLastName(record.lastName);
    setEditUserVisible(true);

  };

  const hideEditUserModal = () => {
    setEditUserVisible(false);
  };

  const handleEditUserModal = () => {
    dispatch(
      {
        type: RELOAD
      }
    )
    dashboardService.handleUserEdit(userId, FirstName, LastName, Age, Email)
      .then(function (response) {
        console.log(response);
        setEditUserVisible(false);
        dispatch(get_page_of_users(currentPage, pageSize, orderColumnName, orderBy, searchString, searchColumn));
        notification.success(
          {
            message: "Success",
            description: "User Edit Success!",
            duration: 2,
            placement: 'bottomRight'
          }
        )
      })
      .catch(function (error) {
        console.log(error);
        setEditUserVisible(false);
        notification.error(
          {
            message: "Error",
            description: "User Edit Error!",
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
    dashboardService.handleUserDelete(id)
      .then(
        () => {
          dispatch(get_page_of_users(currentPage, pageSize, orderColumnName, orderBy, searchString, searchColumn));
          setNumberOfUsers(store.getState().dashboard.numberOfUsers)
          notification.success(
            {
              message: "Success",
              description: "User Deleted Successfully!",
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
              description: "User wasn't deleted!",
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
    dispatch(get_page_of_users(currentPage, pageSize, sorter.field, sorter.order, searchString, searchColumn))

  }

  const handleChangeOfPage = (pageNumber, ColumnName, OrderBy, SearchString, SearchColumn) => {
    dispatch(
      {
        type: RELOAD
      }
    )
    setOrderBy(OrderBy);
    setCurrentPage(pageNumber);
    setOrderColumnName(ColumnName);
    setSearchString(SearchString);
    setSearchColumn(SearchColumn);
    dispatch(get_page_of_users(pageNumber, pageSize, ColumnName, OrderBy, SearchString, SearchColumn));


  }

  // Початкове завантаження сторінки користувачів
  useEffect(() => {
    dispatch(get_page_of_users(currentPage, pageSize, orderColumnName, orderBy, searchString, searchColumn));
  }, [])

  useEffect(() => {
    setNumberOfUsers(store.getState().dashboard.numberOfUsers)
    setCurrentPage(currentPage)
  }, [store.getState().dashboard.userList, store.getState().dashboard.numberOfUsers, currentPage, numberOfUsers])

  // Якщо сторінка порожня (наприклад, після видалення останнього користувача на сторінці) - перейти на попередню сторінку
  useEffect(
    () => {
      if (userList.length === 0 && currentPage > 1) {
        setCurrentPage(currentPage - 1)
        dispatch(get_page_of_users(currentPage - 1, pageSize, orderColumnName, orderBy, searchString, searchColumn));

        setNumberOfUsers(store.getState().dashboard.numberOfUsers)
      }
    }, [userList]
  )

  const dashboardColumns = [
    {
      title: "First Name",
      dataIndex: 'firstName',
      key: 'firstName',
      sorter: true,
      sortDirections: ['ascend', 'descend', 'ascend']
    },
    {
      title: "Last Name",
      dataIndex: 'lastName',
      key: 'lastName',
      sorter: true,
      sortDirections: ['ascend', 'descend', 'ascend']
    },
    {
      title: "Email",
      dataIndex: 'email',
      key: 'email',
      sorter: true,
      sortDirections: ['ascend', 'descend', 'ascend']
    },
    {
      title: "Age",
      dataIndex: 'age',
      key: 'age',
      sorter: true,
      sortDirections: ['ascend', 'descend', 'ascend']
    },
    {
      title: "Registered Date",
      dataIndex: 'registeredDate',
      key: 'registeredDate',
      sorter: true,
      sortDirections: ['ascend', 'descend', 'ascend']
    },

    {
      title: 'Action',
      key: 'action',
      render: (record) => (
        <Space align="center">
          <Button
            size="middle"
            type="primary"
            onClick={() => showEditUserModal(record)}
          >
            Edit user
          </Button>

          <Popconfirm
            title="Delete this user?"
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

  const expandedColumns = [
    {
      title: "Course name",
      dataIndex: ["course", "courseName"],
      key: 'courseName',
      sortDirections: ['ascend', 'descend', 'ascend'],
    },
    {
      title: "Course Description",
      dataIndex: ["course", "courseDescription"],
      key: 'courseName',
      sortDirections: ['ascend', 'descend', 'ascend'],
    },
    {
      title: "Study Date",
      dataIndex: "studyDate",
      key: 'courseName',
      sortDirections: ['ascend', 'descend', 'ascend'],
      render: (record) => record.split('T')[0]
    }
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
      {isAdmin ? <div>
        <Modal
          visible={editUserVisible}
          title="Edit user"
          closable={false}
          onOk={handleEditUserModal}
          onCancel={hideEditUserModal}
        ><br />
          <Form>
            <Space direction="vertical" align="start">
              <Space direction="vertical">
                <Text>First Name</Text>
                <Form.Item>
                  <Input
                    type="text"
                    value={FirstName}
                    placeholder="First Name"
                    onChange={
                      event => setFirstName(event.target.value)
                    }
                    style={{ width: '400px' }}
                  />
                </Form.Item>
              </Space>
              <Space direction="vertical">
                <Text>Last Name</Text>
                <Form.Item>
                  <Input
                    type="text"
                    value={LastName}
                    placeholder="Last Name"
                    onChange={
                      event => setLastName(event.target.value)
                    }
                    style={{ width: '400px' }}
                  />
                </Form.Item>
              </Space>
              <Space direction="vertical">
                <Text>Email </Text>
                <Form.Item>

                  <Input
                    type="text"
                    value={Email}
                    placeholder="Email"
                    onChange={event => setEmail(event.target.value)}
                    style={{ width: '400px' }}
                  />
                </Form.Item>
              </Space>
              <Space direction="vertical">
                <Text>Age</Text>
                <Form.Item>
                  <Input
                    type="number"
                    value={Age}
                    placeholder="Age"
                    onChange={event => setAge(event.target.value)}
                    style={{ width: '400px' }}
                  />
                </Form.Item>
              </Space>
            </Space>
          </Form>
        </Modal>

        <Row>
          <Col offset={1} span={22}>
            <Select
              labelInValue
              defaultValue={{ value: searchColumn }}
              style={{ width: 120 }}
              onChange={(value) => changeSearchColumn(value.value)}
            >
              <Option value="FirstName">First Name</Option>
              <Option value="LastName">Last Name</Option>
              <Option value="Email">Email</Option>
            </Select>
            <Divider type="vertical"></Divider>
            <Search placeholder="Search" allowClear onSearch={(string) => {
              handleChangeOfPage(currentPage, orderColumnName, orderBy, string, searchColumn)
            }
            } style={{ width: 200 }} /><br /><br />
            <Spin
              indicator={antIcon}
              size="large"
              spinning={isLoading}>
              <Table
                dataSource={userList}
                columns={dashboardColumns}
                pagination={false}
                onChange={handleSorting}
                onExpand={(expanded, record) => { handleExpand(expanded, record); }}
                rowKey="id"
                expandedRowKeys={expandedKey}
                expandable={
                  {

                    rowExpandable: record => record.userCourses.length !== 0,
                    expandedRowRender: (record) =>
                      <Table
                        dataSource={record.userCourses}
                        columns={expandedColumns}
                        pagination={false}
                        bordered

                      />
                  }
                }

              />
            </Spin>
            <br />
            <Pagination сurrent={currentPage || 1}
              pageSize={5}
              total={numberOfUsers}
              onChange={
                (page) => handleChangeOfPage(page, orderColumnName, orderBy, searchString)
              }
            />
          </Col>
        </Row>
      </div>
        :
        <Redirect exact to="/401" />}</>
  );

}