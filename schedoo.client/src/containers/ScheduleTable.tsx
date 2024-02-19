import {useEffect, useState } from "react";
import FiberManualRecordIcon from '@mui/icons-material/FiberManualRecord';
import {ScheduleDate, Schedule, ScheduleViewData, TimeSlot, ScheduleAll} from "../types/interfaces.ts";
import {ScheduleRow} from "../components/ScheduleRow.tsx";
import { WeekType } from "../types/enums.ts";
import { CircularProgress } from "@mui/material";
import axios from "../api/axios.ts";
import { AxiosResponse, isAxiosError } from "axios";
import { getScheduleByGroupNameReq } from "../api/requests.ts";

export function ScheduleTable(props: { 
    semesterId: number, 
    weekType: WeekType, 
    selectedGroupId: string
}) {
    const [scheduleAllData, setScheduleAllData] = useState<ScheduleAll>();
    const [currentSchedule, setCurrentSchedule] = useState<Schedule[]>();

    const [dates, setDates] = useState<ScheduleDate[]>();
    const [isEmptySchedule, setIsEmptySchedule] = useState<boolean>(true);
    const [timeSlots, setTimeSlots] = useState<TimeSlot[] | undefined>();
    const [days, setDays] = useState<string[]>();
    const [currentDate, setCurrentDate] = useState<Date>(new Date());
    const currentDay = (days !== undefined) ? days[currentDate.getDay() - 1] : undefined;

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

    return (
        isEmptySchedule ? (
            <span>Empty schedule</span>
        ) : loading ? (
            <CircularProgress/>
        ) : (
            <table className="table table-bordered">
                <thead>
                    <tr>
                        <th></th>
                        { days.map((day, index) => (
                            <th
                                key={index}
                            >{day === currentDay ? blinkingDotForCurrentDay(day) : day}</th>
                        ))}
                    </tr>
                </thead>
                <tbody>
                    { timeSlots.map((timeSlot: TimeSlot) => {
                        const data: ScheduleViewData = { 
                            scheduleData: currentSchedule, 
                            weekType: props.weekType, 
                            timeSlot: timeSlot, 
                            days, 
                            dates,
                            currentDate 
                        }
                        return <ScheduleRow key={timeSlot.id} data={data} />
                    })}
                </tbody>
            </table>
        )
    );
    
    // async function populateScheduleData() {
    //     if (props.selectedGroupId) {
    //         try {
    //             const response = await fetch('groupschedule/groupName=' + props.selectedGroupId);

    //             if (!response.ok) {
    //                 throw new Error(`HTTP error! Status: ${response.status}`);
    //             }

    //             const { dates, days, scheduleAll, timeSlots } = await response.json();

    //             if (scheduleAll.oddWeekSchedule) {
    //                 setScheduleAllData(scheduleAll);
    //                 setCurrentSchedule(props.weekType === WeekType.Odd
    //                     ? scheduleAll?.oddWeekSchedule
    //                     : scheduleAll?.evenWeekSchedule);
    //                 setTimeSlots(timeSlots);
    //                 setDays(days);
    //                 setDates(dates);
    //                 setIsEmptySchedule(false);
    //             } else {
    //                 setIsEmptySchedule(true);
    //             }
    //         } catch (error) {
    //             console.error("Error fetching schedule data", error);
    //         } finally {
    //             setLoading(false); // Set loading to false after the asynchronous operation completes
    //         }
    //     } else {
    //         setIsEmptySchedule(true);
    //     }
    // }

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
                    setDays(days);
                    setDates(dates);
                    setIsEmptySchedule(false);
                } else {
                    setIsEmptySchedule(true);
                }
                
            } catch (err: any) {
                if (isAxiosError(err)) {
                    // Handle Axios-specific errors
                    console.error("Axios error:", err.message);
                } else {
                    // Handle general errors
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