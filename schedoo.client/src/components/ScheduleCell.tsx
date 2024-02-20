import { useNavigate } from "react-router-dom";
import { Class, ScheduleDate } from "../types/interfaces";

export function ScheduleCell({ isActive, isAttendanceAllowed, classData, scheduleDate }
    : { isActive: boolean, isAttendanceAllowed: boolean, classData: Class, scheduleDate: ScheduleDate }) {
    const navigate = useNavigate();

    function handleClick() {
        1
        if (scheduleDate) {
            navigate(`/attendance/${scheduleDate.id}`);
        }
    }

    return (
        <td className={isActive && isAttendanceAllowed ? 'active-card' : ''} >
            {classData && 
                <div className="class-card">
                    <div className="class-content">
                        <div className="class-header">
                            <p className="class-title">
                                {classData.name}
                            </p>
                            <i>{classData.teacher.surname} {classData.teacher.name}</i>
                        </div>
                        <div className="d-flex">
                            <div className="class-info">
                                <p className='class-type'>#{classData.lessonType.toLowerCase()}</p>
                                <p className="class-room">{classData.room.name}</p>
                            </div>
                            { isAttendanceAllowed &&
                                <div>
                                    <button
                                        onClick={handleClick}></button>
                                </div>
                            }
                        </div>
                    </div>
                </div>
            }
        </td>
    )
}