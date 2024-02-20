import {ScheduleCell} from "./ScheduleCell.tsx";
import {dayOfWeekToString, isInTimeSlot} from "../types/helpers.ts";
import { Class, ScheduleDate, Schedule, ScheduleViewData } from "../types/interfaces.ts";

export function ScheduleRow({data} : {data: ScheduleViewData}) {
    
    var scheduleSlot = (data.days ?? []).map((day: string) => {
        const daySchedule = (data.scheduleData ?? [])
            .find((sd: Schedule) => dayOfWeekToString(sd.dayOfWeek - 1) === day.day
                && sd.timeSlotId === data.timeSlot.id
            )
 
        const scheduleDate = data.dates.find((sd: ScheduleDate) => sd.scheduleId === daySchedule?.id);

        const isActiveClass = (data.days && data.currentDate && (data.days[data.currentDate.getDay() - 1] === day))
            && isInTimeSlot(data.currentDate, data.timeSlot);

        const classData: Class = daySchedule?.class;
        return <ScheduleCell 
            key={day.day} 
            isActive={isActiveClass ?? false} 
            isAttendanceAllowed={data.currentWeekType === data.weekType}
            classData={classData} 
            scheduleDate={scheduleDate} 
        />;
    })
    
    return (
        <tr>
            <td className="timeslot-cell">
            <div>
                <p>{data.timeSlot.startTime.toString().slice(0, -3)}</p>
            <div className='line'></div>
        <p>{data.timeSlot.endTime.toString().slice(0, -3)}</p>
        </div>
        </td>
        {scheduleSlot}
    </tr>
);
};
