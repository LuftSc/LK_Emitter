import Modal from "antd/es/modal"
import { useState } from "react"
import { LoginUserRequest } from "../services/users";
import { EyeInvisibleOutlined, EyeTwoTone } from '@ant-design/icons';
import { Button, Input } from "antd";

interface Props {
    isModalOpen: boolean
    handleLogin: (request: LoginUserRequest) => void;
    handleCancel: () => void;
}

export const LoginUser = ({
        isModalOpen, 
        handleLogin,
        handleCancel
    } : Props) => {

    const [email, setEmail] = useState<string>("tema.prizrak90@mail.ru");
    //const [passwordVisibile, setPasswordVisibile] = useState<boolean>(true);
    const [password, setPassword] = useState<string>("123");
    const [loading, setLoading] = useState(false);
    
    const handleOnOk = async () => {
        const loginUserRequest = {
            email: email,
            password: password
        } as LoginUserRequest
        
        handleLogin(loginUserRequest);
    }

    return (
        <Modal 
            title="Войдите в свой аккаунт: "
            open={isModalOpen}
            cancelText="Отмена"
            onOk={handleOnOk}
            onCancel={handleCancel}
            width='350px'
            footer={[
                <Button key="back" size='large' onClick={handleCancel}>
                  Отмена
                </Button>,
                <Button key="submit" type="primary" size='large' loading={loading} onClick={handleOnOk}>
                  Войти
                </Button>
                ]
            }
        > 
            <div style={{display: "flex", flexDirection: "column", gap: "20px"}}>
                <div style = {{display: "flex", flexDirection: "column", marginTop: "30px"}}>
                    <p>Введите вашу почту</p>
                    <Input 
                        value = {email}
                        onChange = {(e:any) => setEmail(e.target.value)}
                        placeholder='my.email@mail.ru'
                        style={{"width": "250px"}}
                    />
                </div>
                <div style = {{display: "flex", flexDirection: "column", marginBottom: "30px"}}>
                    <p>Введите пароль</p>
                    <Input.Password
                        value = {password}
                        placeholder="Мой супер-пароль 123"
                        iconRender={(visible: any) => (visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />)}
                        onChange = {(e:any) => {
                            setPassword(e.target.value)
                        }}
                        style={{"width": "250px"}}
                    />                
                </div>
            </div>
        </Modal>
    )
}