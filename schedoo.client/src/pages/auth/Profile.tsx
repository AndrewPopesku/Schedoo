import { useEffect, useState } from "react";
import { useAuth } from "../../hooks/useAuth"
import { User } from "../../types/interfaces";
import axios, { AxiosResponse, isAxiosError } from "axios";

export function Profile() {
    const { auth } = useAuth();
    const [user, setUser] = useState<User>();

    useEffect(() => {
        populateUserData();
    }, [])

    return (
        <>
            <div className="profile-container">
                <div className="user-info">
                    {/* <img
                        className="profile-picture"
                        src={user?.profilePictureUrl}
                        alt="Profile"
                    /> */}
                    {/* <button className="change-picture-button">Change Picture</button> */}
                    <table className="profile-table">
                        <tr>
                            <th>Username:</th>
                            <td>{user?.username}</td>
                        </tr>
                        <tr>
                            <th>Name:</th>
                            <td>{user?.name}</td>
                        </tr>
                        <tr>
                            <th>Surname:</th>
                            <td>{user?.surname}</td>
                        </tr>
                        <tr>
                            <th>Patronymic:</th>
                            <td>{user?.patronymic}</td>
                        </tr>
                        <tr>
                            <th>Phone number:</th>
                            <td>{user?.phoneNumber ?? "no phone number"}</td>
                        </tr>
                        
                    </table>
                    <br />
                    <hr />
                    <p>Attended classes: {user?.attendancesPresent}/{user?.attendancesTotal}</p>
                </div>
            </div>
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