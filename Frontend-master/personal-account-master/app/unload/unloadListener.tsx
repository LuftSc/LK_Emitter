"use client"

import { useEffect, useState } from "react";
import { logUserLogIn } from "../services/usersService";

export function UnloadListener() {
    // Удаляем метку при загрузке страницы (значит, это обновление)
    useEffect(() => {
        const handleBeforeUnload = (event: BeforeUnloadEvent) => {
            navigator.sendBeacon('http://localhost:5000/usersActions/logout');
        };
        window.addEventListener("beforeunload", handleBeforeUnload);

        const logActivity = async () => {
            console.log('зашли')
            await logUserLogIn();
        }

        const sessionInfo = sessionStorage.getItem('activeSession')
            if (!sessionInfo) {
            console.log('зашли')
            logActivity();
            //await logUserLogIn()
            sessionStorage.setItem('activeSession', 'active')
        }
        

        return () => {
            window.removeEventListener("beforeunload", handleBeforeUnload);
        };
    }, []);

    return null; 
}