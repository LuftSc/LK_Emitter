"use client"

import Image from "next/image";
import styles from "./page.module.css";
import { useEffect, useState } from "react";
import { Button, Table } from "antd";
import { downloadDocument, getAllDocumentsByUserId } from "./services/documents";
import { DocumentInfo } from "./models/Document";
import { DownloadLink } from "./components/DownloadLink";
import { LoginUser } from "./components/LoginUser";
import { getCurrentUserId, loginUser, LoginUserRequest } from "./services/users";
import { useSignalR } from "./components/SignalRContext";

const columns = [
  {
    title: 'Название',
    dataIndex: 'title',
    key: 'title',
  },
  {
    title: 'Тип файла',
    dataIndex: 'fileExtnsion',
    key: 'fileExtnsion',
  },
  {
    title: 'Дата загрузки',
    dataIndex: 'uploadDate',
    key: 'uploadDate',
  },
  {
    title: 'Размер файла',
    dataIndex: 'size',
    key: 'size',
  },
  {
    title: "Скачать",
    key: "download",
    render: (text:string, record: DocumentInfo) => (
      <DownloadLink documentInfo={record}  />
    ),
  },
];


export default function Home() {
  const [userId, setUserId] = useState<string>("ca894255-3270-41ca-9c0e-69b01560a032");
  const [documents, setDocuments] = useState<DocumentInfo[]>([])
  const [isLoginModalOpen, setIsLoginModalOpen] = useState<boolean>(true)

  const { startConnection } = useSignalR();
  
  useEffect(() => {
    const getDocuments = async () => {
        const documents = await getAllDocumentsByUserId(userId);

        if (typeof(documents) != 'undefined') {
            setDocuments(documents)
        } 
    };
    getDocuments();
  }, [])

  const onHandleLogin = async (request: LoginUserRequest) => {
    await loginUser(request)

    setIsLoginModalOpen(false)
    await getCurrentUserId()
    startConnection()
  }

  const onDownloadDocument = async() => {
    await downloadDocument('ac5301c9-6e2c-4050-a6fe-445e3c711e4e')
  }

  return (
    <div>
      <LoginUser isModalOpen={isLoginModalOpen} handleLogin={onHandleLogin} handleCancel={() => setIsLoginModalOpen(false)} />
      <Table rowKey="id" dataSource={documents} columns={columns} />
      
    </div>
  );
}
