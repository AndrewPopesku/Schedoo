import {WeekType} from "./enums.ts";

export interface Semester {
    id: number,
    description: string;
    startDay: string;
    endDay: string;
    currentSemester: boolean;
}

export interface Group {
    id: string,
    name: string
}

export interface TimeSlot {
    id: number,
    className: string,
    startTime: string,
    endTime: string,
}

export interface Schedule {
    id: number,
    weekType: WeekType,
    timeSlotId: number,
    dayOfWeek: number,
    classId: string,
    groupId: string,
    class: Class
}

export interface Class {
    id: string,
    name: string,
    lessonType: string,
    linkToMeeting: string,
    room: Room,
    teacher: Teacher,
}

export interface Room {
    id: string,
    name: string
}

export interface Teacher {
    id: string,
    name: string,
    surname: string,
    patronymic: string,
    position: string
}

export interface Teacher {

}

export interface ScheduleAll {
    oddWeekClasses: Schedule[],
    evenWeekClasses: Schedule[],
}

export interface ScheduleViewData {
    scheduleData: Schedule[] | undefined,
    weekType: WeekType,
    timeSlot: TimeSlot,
    days: string[] | undefined,
    dates: ScheduleDate[],
    currentDate: Date,
}

export interface ScheduleDate {
    id: number,
    date: Date,
    scheduleId: number
}

export interface Attendance {
    id: number,
    scheduleDate: ScheduleDate,
    student: User,
    attendanceStatus: AttendanceStatus
}

export enum AttendanceStatus {
    Present,
    Absent
}

export interface User {
    id: string,
    userName: string,
    email: string
}