import { Card } from 'antd';
import { MehOutlined } from '@ant-design/icons';
const Error_401 = () => {
    return (
        <div align="center">
            <Card style={{ width: "500px" }}>
                <MehOutlined style={{ fontSize: 56 }} />
                <br></br>
                Sorry, but your'e not authorized to access this page
            </Card>
        </div>
    )
}

export default Error_401