import { useState } from "react"
import { Attendance, AttendanceStatus } from "../types/interfaces"
import axios from "../api/axios";
import { AxiosResponse, isAxiosError } from "axios";
import { getAttendanceByIdReq } from "../api/requests";



export function AttendanceRow({ attendance }
    : {
        attendance: Attendance
    }) {
    const [status, setStatus] = useState<AttendanceStatus>(attendance.attendanceStatus);

    return (
        <tr>
            <td>{attendance.studentName} {attendance.studentSurname} {attendance.studentPatronymic}</td>
            <td>
                <select
                    title="Select attendance status"
                    value={status}
                    onChange={(e) => handleStatusChange(Number(e.target.value) as AttendanceStatus)}
                >
                    <option value={AttendanceStatus.Present}>Present</option>
                    <option value={AttendanceStatus.Absent}>Absent</option>
                </select>
            </td>
        </tr>
    )

    function handleStatusChange(statusToChange: AttendanceStatus) {
        setStatus(statusToChange);
        updateAttendanceStatus(statusToChange);
    };

    async function updateAttendanceStatus(statusToChange: AttendanceStatus) {
        try {
            const response: AxiosResponse = await axios.put(getAttendanceByIdReq(attendance.id), statusToChange,
                {
                    headers: { 'Content-Type': 'application/json' },
                    withCredentials: true
                });
            return response;
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