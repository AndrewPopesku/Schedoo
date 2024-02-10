import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axios from "../api/axios";
import { AxiosResponse, isAxiosError } from "axios";
import { Attendance } from "../types/interfaces";
import { getStatusString } from "../types/helpers";

const ATD = 'attendance/get/';

export function AttendancePage() {
    const { scheduleDateId } = useParams();
    const [attendances, setAttendances] = useState<Attendance[]>();

    useEffect(() => {
        populateAttendanceData();
    }, []);

    const table = 
        <table>
            <thead>
                <th>Student</th>
                <th>Status check</th>
            </thead>
            <tbody>
                {attendances?.map((attendance) => (
                    <tr>
                        <td>{attendance.student.userName}</td>
                        <td>{getStatusString(attendance.attendanceStatus)}</td>
                    </tr>
                ))}
            </tbody>
        </table>

    return attendances ? table : "nothing";

    async function populateAttendanceData() {
        try {
            const response: AxiosResponse = await axios.get(ATD + scheduleDateId);
            const responseData: Attendance[] = response.data;
            console.log(responseData);
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