import { notification } from 'antd';
import { useEffect } from 'react'
import { Redirect, useParams } from 'react-router-dom'
import accountService from '../../services/accountService';
const EmailConfirmation = () => {
    const { userid, code } = useParams();

    const ConfirmEmail = () => {
        accountService.handleEmailConfirmation(userid, code)
            .then(
                () => {
                    notification.success(
                        {
                            message: "Success",
                            description: "Email Confirmed!"
                        }
                    )
                    
                    return (
                        <Redirect exact to="/" />

                    )
                }
            ).catch(
                () => {
                    notification.error(
                        {
                            message: "Error",
                            description: "Email Confirmation Error"
                        }
                    )

                }
            )
    }
    useEffect(ConfirmEmail, [])
    return (
        <p>Email Confirmation page
            {() => ConfirmEmail()}
            <Redirect to="/" />
        </p>

    )

}

export default EmailConfirmation