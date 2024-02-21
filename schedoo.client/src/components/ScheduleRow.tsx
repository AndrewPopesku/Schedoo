import { ScheduleCell } from "./ScheduleCell.tsx";
import { dayOfWeekToString, isInTimeSlot } from "../types/helpers.ts";
import { Class, ScheduleDate, Schedule, TimeSlot, DayDate } from "../types/interfaces.ts";
import { WeekType } from "../types/enums.ts";
import { useContext } from "react";
import { IsCurrentWeekContext } from "../pages/SchedulePage.tsx";

export function ScheduleRow(props
    : {
        scheduleData: Schedule[],
        weekType: WeekType,
        timeSlot: TimeSlot,
        dayDates: DayDate[],
        scheduleDates: ScheduleDate[],
        currentDate: Date
    }) {
    
    const isCurrentWeekType = useContext(IsCurrentWeekContext)

    var scheduleRow = props.dayDates?.map((dayDate: DayDate) => {
        const daySchedule = props.scheduleData?.find((sd: Schedule) =>
            dayOfWeekToString(sd.dayOfWeek - 1) === dayDate.day && sd.timeSlotId === props.timeSlot.id
        )
        const scheduleDate = props.scheduleDates.find((sd: ScheduleDate) =>
            sd.scheduleId === daySchedule?.id
        );
        const isActiveClass = (props.dayDates && props.currentDate
            && (props.dayDates[props.currentDate.getDay() - 1] === dayDate))
            && isInTimeSlot(props.currentDate, props.timeSlot)
            && isCurrentWeekType;
        const classData: Class = daySchedule?.class;

        return <ScheduleCell
            key={dayDate.day}
            isActive={isActiveClass}
            classData={classData}
            scheduleDate={scheduleDate}
        />;
    })

    return (
        <tr>
            <td className="timeslot-cell">
                <div>
                    <p>{props.timeSlot.startTime.toString().slice(0, -3)}</p>
                    <div className='line'></div>
                    <p>{props.timeSlot.endTime.toString().slice(0, -3)}</p>
                </div>
            </td>
            {scheduleRow}
        </tr>
    );
};
