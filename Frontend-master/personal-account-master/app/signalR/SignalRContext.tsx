"use client"

import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr"
import { createContext, useContext, useEffect, useState } from "react"

type SignalRContextType = {
    connection: HubConnection | null
    startConnection : () => Promise<HubConnection | undefined>
    stopConnection: () => void
}

// Создаём контекст
const SignalRContext = createContext<SignalRContextType | null>(null);

export const SignalRProvider = ({children} : {children: React.ReactNode}) => {
    const [connection, setConnection] = useState<HubConnection | null>(null);

    // Функция для запуска соединения
    const startConnection = async () : Promise<HubConnection | undefined> => {
        if (typeof window === 'undefined') return;
        
        const newConnection = new HubConnectionBuilder()
            .withUrl("http://localhost:5000/resultsHub")
            .withAutomaticReconnect()
            .build();

        try {
            await newConnection.start();
            setConnection(newConnection);
            return newConnection;
        } catch (error) {
            console.log("Ошибка подключения к SignalR:", error);
        }

        //return newConnection;
    }
    

    // Функция для остановки соединения
    const stopConnection = async () => {
        if (connection) {
            await connection.stop();
            console.log("Соединение с SignalR закрыто.");
            setConnection(null);
        }
    };

    // Очистка при размонтировании
    useEffect(() => {
        return () => {
            stopConnection();
        };
    }, []); 

    return (
        <SignalRContext.Provider value={{connection, startConnection, stopConnection}}>
            {children}
        </SignalRContext.Provider>
    )
}

// Хук для использования SignalR в компонентах
export const useSignalR = () => {
    const context = useContext(SignalRContext);
    if (!context) {
        throw new Error("useSignalR must be used within a SignalRProvider");
    }
    return context;
}