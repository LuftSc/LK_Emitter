"use client"

import React, { useState } from 'react';
import { DeleteOutlined, DownloadOutlined, InboxOutlined, UploadOutlined } from '@ant-design/icons';
import { Button, message, Upload } from 'antd';
import type { GetProp, UploadFile, UploadProps } from 'antd';
import { uploadDocuments } from '@/app/services/documentsService';
import Dragger from 'antd/es/upload/Dragger';
import { useSimpleStorage } from '@/app/hooks/useLocalStorage';

type FileType = Parameters<GetProp<UploadProps, 'beforeUpload'>>[0];



export const UploadDocumentArea = () => {
  const [fileList, setFileList] = useState<UploadFile[]>([]);
  const [uploading, setUploading] = useState(false);
  const [willSign, setWillSign] = useState(false);
  const [emitterInfo, setEmitterInfo] = useState<{
    Id: string, 
    Name: string, 
    AuthPerson: string}>
    ({Id: "", Name: "", AuthPerson: ""})

  const handleUpload = async () => {
    const { getItem } = useSimpleStorage('emitter');
    setEmitterInfo(getItem())

    const formData = new FormData();
    fileList.forEach((file) => {
      formData.append('Files', file as FileType);
    });
      // 2. Добавляем остальные параметры в FormData (не в URL!)
    
    formData.append('SenderId', emitterInfo.AuthPerson );
    formData.append('EmitterId', emitterInfo.Id);
    formData.append('WithDigitalSignature', String(willSign));
    

    setUploading(true);
    
    const response = await uploadDocuments(formData)

    if (response?.ok) {
      setFileList([]);
      message.success('upload successfully.');
    } else {
      message.error('upload failed.')
    }
    setUploading(false)
    setWillSign(false)
  };

  const props: UploadProps = {
    listType: 'picture',
    multiple: true,
    onRemove: (file) => {
      const index = fileList.indexOf(file);
      const newFileList = fileList.slice();
      newFileList.splice(index, 1);
      setFileList(newFileList);
    },
    beforeUpload: (file) => {
      setFileList(prev => [...prev, file]);

      return false;
    },
    fileList,
    
  };
  
  return (
    <>
      <Dragger {...props}
        >
        <p className="ant-upload-drag-icon">
          <InboxOutlined />
        </p>
        <p className="ant-upload-text">Нажмите или перетащите файлы для загрузки</p>
        <p className="ant-upload-hint">
          Поддерживается загрузка одного или нескольких файлов
        </p>
      </Dragger>

    <Button
        type="primary"
        onClick={handleUpload}
        disabled={fileList.length === 0}
        loading={uploading}
        style={{ marginTop: 16 }}
      >
        {uploading ? 'Отправляем ваши файлы' : 'Отправить файлы'}
    </Button>
    <Button
        type={willSign ? 'primary' : 'default'}
        onClick={() => setWillSign(!willSign)}
        disabled={fileList.length === 0}
        style={{
          background: willSign ? '#000' : undefined,
          borderColor: willSign ? '#000' : undefined,
          color: willSign ? '#fff' : undefined,
          marginLeft: 20
        }}
      >
        {willSign ? 'Документы будут подписаны' : 'Подписать ЭЦП'}
      </Button>
  </>
    
  );
};

//export default UploadDocumentArea;