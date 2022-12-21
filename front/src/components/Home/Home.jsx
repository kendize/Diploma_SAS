import { Card, Row, Col, Image } from 'antd';

const Home = () => {
    return (
        <>
            <Row>
                <Col span={12} offset={1}>
                    <Card
                        hoverable
                        title={<h1>Student Accounting System</h1>}
                        style={{ borderRadius: "25px" }}
                        bodyStyle={{ margin: '12px' }}
                        headStyle={{ backgroundColor: "#e4e0ff", borderRadius: "25px" }}>
                            <div align = "center">
                        <Image
                            src="https://cdn2.avada.io/media/resources/TGpWB6I.jpg"
                            preview={false}
                            width={'60%'}
                            style={{ borderRadius: '15px' }}
                        /></div>
                        <h2>Online Platform</h2>
                        <h3>Start learning from the world’s best institutions - from home!</h3>

                        <h3>As a global nonprofit, we're relentlessly pursuing our vision of a world, where every learner can access education to unlock their potential, without the barriers of cost or location.
                        </h3>
                    </Card>
                    <br />

                </Col>
                <Col span={8} offset={1}>
                    <Card
                        hoverable
                        title={<h2>Advantages:</h2>}
                        style={{ borderRadius: "25px" }}
                        bodyStyle={{ margin: '12px' }}
                        headStyle={{ backgroundColor: "#d9ffd6", borderRadius: "25px" }}
                    >

                        <ul>
                            <li>
                                <h3>Online Education</h3>
                            </li>
                            <li>
                                <h3>Variety of courses</h3>
                            </li>
                            <li>
                                <h3>Free of charge</h3>
                            </li>
                            <li>
                                <h3>E-mail Notifications about start of study</h3>
                            </li>

                        </ul>
                    </Card>
                </Col>
            </Row>
            <Row>
                <Col>

                </Col>
            </Row>

        </>
    )
}

export default Home