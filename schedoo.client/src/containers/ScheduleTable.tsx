import {useContext, useEffect, useState } from "react";
import FiberManualRecordIcon from '@mui/icons-material/FiberManualRecord';
import {ScheduleDate, Schedule, TimeSlot, ScheduleAll, DayDate} from "../types/interfaces.ts";
import {ScheduleRow} from "../components/ScheduleRow.tsx";
import { WeekType } from "../types/enums.ts";
import { CircularProgress } from "@mui/material";
import { AxiosResponse, isAxiosError } from "axios";
import axios from "../api/axios.ts";
import { getScheduleByGroupNameReq } from "../api/requests.ts";
import { IsCurrentWeekContext } from "../pages/SchedulePage.tsx";

export function ScheduleTable(props
    : { 
        semesterId: number, 
        weekType: WeekType, 
        isAttendanceAllowedToChange: boolean,
        selectedGroupId: string
}) {
    const isCurrentWeekType = useContext(IsCurrentWeekContext);

    const [scheduleAllData, setScheduleAllData] = useState<ScheduleAll>();
    const [currentSchedule, setCurrentSchedule] = useState<Schedule[]>();

    const [dayDates, setDayDates] = useState<DayDate[]>();
    const [scheduleDates, setScheduleDates] = useState<ScheduleDate[]>();
    const [timeSlots, setTimeSlots] = useState<TimeSlot[]>();
    const [currentDate, setCurrentDate] = useState<Date>(new Date());
    const currentDay = dayDates?.map((d: DayDate) => d.day)[currentDate.getDay() - 1];
    
    const [isEmptySchedule, setIsEmptySchedule] = useState<boolean>(true);
    const [loading, setLoading] = useState<boolean>(false);

    useEffect(() => {
        const interval = setInterval(() => {
            setCurrentDate(new Date());
        }, 5000);
        
        return () => clearInterval(interval);
    }, []);
    
    useEffect(() => {
        setLoading(true);
        populateScheduleData();
    }, [props.selectedGroupId]);

    useEffect(() => {
        setCurrentSchedule(props.weekType === WeekType.Odd
            ? scheduleAllData?.oddWeekSchedule
            : scheduleAllData?.evenWeekSchedule);
    }, [props.weekType])

    function blinkingDotForCurrentDay(day: string) {
        return (
            <div className="active-day">
                <FiberManualRecordIcon color="primary" className="blinking-dot"/>
                {day}
            </div>
        )
    }

    function dayToString(day: DayDate) {
        return day.day + " " + day.date;
    }

    const content = (
        <table className="table table-bordered">
            <thead>
                <tr>
                    <th></th>
                    { dayDates?.map((dayDate: DayDate, index: number) => (
                        <th
                            key={index}
                        >{
                            isCurrentWeekType 
                            ? dayDate.day === currentDay 
                                ? blinkingDotForCurrentDay(dayToString(dayDate)) 
                                : dayToString(dayDate)
                            : dayDate.day}</th>
                    ))}
                </tr>
            </thead>
            <tbody>
                { timeSlots?.map((timeSlot: TimeSlot) => {
                    return <ScheduleRow
                        key={timeSlot.id}
                        scheduleData={currentSchedule}
                        weekType={props.weekType}
                        timeSlot={timeSlot}
                        dayDates={dayDates}
                        scheduleDates={scheduleDates}
                        currentDate={currentDate}
                    />
                })}
            </tbody>
        </table>
    )

    return (
        isEmptySchedule ? (
            <span>Empty schedule</span>
        ) : loading ? (
            <CircularProgress/>
        ) : content
    );
    
    async function populateScheduleData() {
        if (props.selectedGroupId) {
            try {
                const response: AxiosResponse = 
                    await axios.get(getScheduleByGroupNameReq(props.selectedGroupId));
                const { dates, days, scheduleAll, timeSlots } = response.data;

                if (scheduleAll.oddWeekSchedule) {
                    setScheduleAllData(scheduleAll);
                    setCurrentSchedule(props.weekType === WeekType.Odd
                        ? scheduleAll?.oddWeekSchedule
                        : scheduleAll?.evenWeekSchedule);
                    setTimeSlots(timeSlots);
                    setDayDates(days);
                    setScheduleDates(dates);
                    setIsEmptySchedule(false);
                } else {
                    setIsEmptySchedule(true);
                }
            } catch (err: any) {
                if (isAxiosError(err)) {
                    console.error("Axios error:", err.message);
                } else {
                    console.error("General error:", err.message);
                }
            }
            finally {
                setLoading(false);
            }
        } else {
            setIsEmptySchedule(true);
        }
    }
}