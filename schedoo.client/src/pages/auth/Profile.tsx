import { useEffect, useState } from "react";
import { useAuth } from "../../hooks/useAuth"
import { User } from "../../types/interfaces";
import axios, { AxiosResponse } from "axios";

export function Profile() {
    const { auth } = useAuth();
    const [user, setUser] = useState<User>();

    useEffect(() => {
        populateUserData();
    }, [])

    return (
        <>
            <p>{user?.username}</p>
            <p>{user?.name}</p>
            <p>{user?.surname}</p>
            <p>{user?.patronymic}</p>
            <p>{user?.phoneNumber ?? "no phone number"}</p>
            <p>Attended classes: {user?.attendancesPresent}/{user?.attendancesTotal}</p>
        </>
    )

    async function populateUserData() {
        try {
            const config = {
                headers: {
                    'Authorization': `Bearer ${auth?.accessToken}`,
                }
            }

            const response: AxiosResponse = await axios.get(
                "https://localhost:7122/account/userprofile",
                config
            );
            
            const user = response.data;
            setUser(user);
        } catch (err: any) {
            if (isAxiosError(err)) {
                // Handle Axios-specific errors
                console.error("Axios error:", err.message);
            } else {
                // Handle general errors
                console.error("General error:", err.message);
            }
        }
    }
}