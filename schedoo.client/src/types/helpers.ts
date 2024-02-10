import { AttendanceStatus, Semester, TimeSlot } from "./interfaces.ts";
import { WeekType } from "./enums.ts";

export function timeStrToDate(timeStr: string) {
    const parts = timeStr.split(":");
    const hours = parseInt(parts[0], 10);
    const mins = parseInt(parts[1], 10);

    const tmpDate = new Date();
    tmpDate.setHours(hours);
    tmpDate.setMinutes(mins);
    tmpDate.setSeconds(0);

    return tmpDate;
}

export function isInTimeSlot(timeDate: Date, timeslot: TimeSlot) {
    return timeDate > timeStrToDate(timeslot.startTime)
        && timeDate < timeStrToDate(timeslot.endTime);
}

export function getCurrentWeekType(semester: Semester) {
    const timeDiff = new Date().getTime() - timeStrToDate(semester.startDay).getTime();
    const daysElapsed = Math.floor(timeDiff / (1000 * 60 * 60 * 24));
    const weeksElapsed = Math.floor(daysElapsed / 7);

    return weeksElapsed % 2 === 0 ? WeekType.Even : WeekType.Odd;
}

export function dayOfWeekToString(dayStr: number) {
    const daysStr = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday'];
    return daysStr[dayStr];
}

export function getStatusString(status: AttendanceStatus): string {
    return status === AttendanceStatus.Present ? "Present" : "Absent";
}