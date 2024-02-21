import { useNavigate } from "react-router-dom";
import { Class, ScheduleDate } from "../types/interfaces";
import { useContext } from "react";
import { IsCurrentWeekContext } from "../pages/SchedulePage";

export function ScheduleCell(props
    : { 
        isActive: boolean, 
        classData: Class, 
        scheduleDate: ScheduleDate 
    }) {

    const isCurrentWeekType = useContext(IsCurrentWeekContext);
    const navigate = useNavigate();

    function handleClick() {
        if (props.scheduleDate) {
            navigate(`/attendance/${props.scheduleDate.id}`);
        }
    }

    return (
        <td className={props.isActive ? 'active-card' : ''} >
            {props.classData && 
                <div className="class-card">
                    <div className="class-content">
                        <div className="class-header">
                            <p className="class-title">
                                {props.classData.name}
                            </p>
                            <i>
                                {props.classData.teacher.surname} {props.classData.teacher.name} {props.classData.teacher.patronymic}
                            </i>
                        </div>
                        <div className="d-flex">
                            <div className="class-info">
                                <p className='class-type'>#{props.classData.lessonType.toLowerCase()}</p>
                                <p className="class-room">{props.classData.room.name}</p>
                            </div>
                            { isCurrentWeekType &&
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