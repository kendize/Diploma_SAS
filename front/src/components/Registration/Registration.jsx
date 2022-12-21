import React, { useState } from 'react';
import { Form, Button, notification, Space, Input, Card} from 'antd';
import { useHistory } from "react-router-dom";
import accountService from '../../services/accountService';
import { SendOutlined } from '@ant-design/icons';

export default function Registration() {
  const history = useHistory();
  const [Email, setEmail] = useState("");
  const [FirstName, SetFirstName] = useState("");
  const [LastName, SetLastName] = useState("");
  const [Age, SetAge] = useState("");
  const [Password, setPassword] = useState("");

  const handleSubmit = () => {
    const data = {
      email: Email,
      firstname: FirstName,
      lastname: LastName,
      age: Age,
      password: Password
    }

    accountService.handleRegistration(data)
      .then((response) => {
        console.log(response);
        notification.success(
          {
            message: "Success",
            description: "Successfully registrated!",
            placement: 'bottomRight'
          }
        );
        notification.info(
          {
            message: "Confirm your E-Mail",
            description: "Check your E-Mail for activation of profile!",
            placement: 'bottomRight'
          }
        )
        history.push("/")
      })
      .catch((error) => {
        console.log(error);
        notification.error(
          {
            message: "Error",
            description: "Account was not registered!"
          }
        );
      });

  }

  return (

    <div align="center">
      <Card style={{ width: "35%", borderRadius: "25px" }}>
      <h2>Registration</h2>
        <Form scrollToFirstError
        labelCol={{ span: 5}}
        wrapperCol={{ span: 16, }}>
          
          <Form.Item
            name="email"
            label = "Email"
            rules={
              [
                {
                  type: 'email',
                  required: true,
                  message: "Please, input valid E-mail!"
                }
              ]
            }>
            <Input
              type="text"

              value={Email}
              placeholder="Email"
              onChange={event => setEmail(event.target.value)}
              style={{ width: '80%' }}
            />
          </Form.Item>
          <Form.Item
            name="password"
            label = "Password"
            type="password"
            rules={
              [
                {
                  required: true,
                  message: "Please, input password!"
                },
                {
                  min: 5,
                  message: "Password has to be at least 5 character"
                }
              ]
            }>
            <Input
              type="password"
              value={Password}
              placeholder="Password"
              onChange={event => setPassword(event.target.value)}
              style={{ width: '80%' }}
            />
          </Form.Item>

          <Form.Item
          name = "first name"
          label = "First Name"
          rules={
            [
              {
                required: true,
                message: "Please, input First Name!"
              }
            ]
          }>
            <Input
              type="text"
              value={FirstName}
              placeholder="First Name"
              onChange={event => SetFirstName(event.target.value)}
              style={{ width: '80%' }}
            />
          </Form.Item>

          <Form.Item
          name = "last name"
          label = "Last Name"
          rules={
            [
              {
                required: true,
                message: "Please, input Last Name!"
              }
            ]
          }>
            <Input
              type="text"
              value={LastName}
              placeholder="Last Name"
              onChange={event => SetLastName(event.target.value)}
              style={{ width: '80%' }}
            />
          </Form.Item>

          <Form.Item
          name = "age"
          label = "Age"
          rules={
            [
              {
                required: true,
                message: "Please, input Age!"
              }
            ]
          }>
            <Input
              type="number"
              value={Age}
              placeholder="Age"
              onChange={event => SetAge(event.target.value)}
              style={{ width: '80%' }}
            />
          </Form.Item>
          <Space>
          <Form.Item>
            <Button
              style={{ borderColor: "black", width : '150px' }}
              size="middle"
              type="primary"
              onClick={handleSubmit}
            >
              <SendOutlined />
              Register
            </Button>
          </Form.Item>
          </Space>
        </Form>
      </Card>
      <br/>
    </div>
  );
}
