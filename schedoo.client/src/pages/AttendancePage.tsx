import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import axios from "../api/axios";
import { AxiosResponse, isAxiosError } from "axios";
import { Attendance } from "../types/interfaces";
import { CircularProgress } from "@mui/material";
import { AttendanceRow } from "../components/AttendanceRow";
import { getAttendancesByScheduleDateIdReq } from "../api/requests";


export function AttendancePage() {
    const { scheduleDateId } = useParams<string>();
    const [attendances, setAttendances] = useState<Attendance[]>();

    useEffect(() => {
        populateAttendanceData();
    }, []);

    const date = attendances?.map(a => a.date)[0];
    const className = attendances?.map(a => a.className)[0];

    var content =
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
            const response: AxiosResponse = await axios.get(getAttendancesByScheduleDateIdReq(scheduleDateId));
            const responseData: Attendance[] = response.data;
            setAttendances(responseData);
        } catch (err: any) {
            if (isAxiosError(err)) {
                console.error("Axios error:", err.message, err.response?.status);
            } else {
                // Handle general errors
                console.error("General error:", err.message);
            }

        }
    }

    
}