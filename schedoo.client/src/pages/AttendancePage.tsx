import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axios from "../api/axios";
import { AxiosResponse, isAxiosError } from "axios";
import { Attendance } from "../types/interfaces";
import { CircularProgress } from "@mui/material";
import { AttendanceRow } from "../components/AttendanceRow";
import { useAuth } from "../hooks/useAuth";

const ATD = 'attendance/get/scheduleDateId=';

export function AttendancePage() {
    const { scheduleDateId } = useParams();
    const {auth} = useAuth();
    const [attendances, setAttendances] = useState<Attendance[]>();

    useEffect(() => {
        populateAttendanceData();
    }, []);

    const date = attendances?.map(a => a.date)[0];
    const className = attendances?.map(a => a.className)[0];

    const content =
        <section>
            <h2>{className}</h2>
            <h4>{date?.toString()}</h4>
            <table>
                <thead>
                    <tr>
                        <th>Student</th>
                        <th>Status check</th>
                    </tr>
                </thead>
                <tbody>
                    {attendances?.map((attendance, index) => (
                        <AttendanceRow key={index} attendance={attendance}/>
                    ))}
                </tbody>
            </table>
        </section>

    return attendances
        ? content
        : <CircularProgress />;

    async function populateAttendanceData() {
        try {
            const config = {
                headers: {
                    'Authorization': `Bearer ${auth?.accessToken}`,
                }
            }

            const response: AxiosResponse = await axios.get(
                "https://localhost:7122/attendance/get/scheduleDateId=" + scheduleDateId,
                config
            );
            const responseData: Attendance[] = response.data;
            setAttendances(responseData);
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