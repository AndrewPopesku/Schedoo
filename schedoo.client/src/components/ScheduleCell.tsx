import { useNavigate } from "react-router-dom";
import { Class, ScheduleDate } from "../types/interfaces";

export function ScheduleCell({ isActive, classData, scheduleDate }
    : {isActive: boolean, classData: Class, scheduleDate: ScheduleDate}) {
    const navigate = useNavigate();

    function handleClick() {
        navigate(`/attendance/${scheduleDate.id}`);
    }

    return (
        <td className={isActive ? 'active-card' : ''} 
            onClick={handleClick}>
        {classData &&
        <div className="class-card">
            <div className="class-content">
                <div className="class-header">
                    <p className="class-title">
                        {classData.name}
                    </p>
                    <i>{classData.teacher.surname} {classData.teacher.name}</i>
                </div>
                <div className="class-info">
                    <p className='class-type'>#{classData.lessonType.toLowerCase()}</p>
                    <p className="class-room">{classData.room.name}</p>
                </div>
            </div>
        </div>
}
    </td>
)
}