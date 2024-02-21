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
    oddWeekSchedule: Schedule[],
    evenWeekSchedule: Schedule[],
}

export interface ScheduleViewData {
    scheduleData: Schedule[] | undefined,
    weekType: WeekType,
    timeSlot: TimeSlot,
    days: string[] | undefined,
    dates: ScheduleDate[],
    currentDate: Date,
    isAttendanceAllowedToChange: boolean,
    currentWeekType: WeekType
}

export interface ScheduleDate {
    id: number,
    date: Date,
    scheduleId: number
}

export interface Attendance {
    id: number,
    date: Date,
    studentName: string,
    studentSurname: string,
    studentPatronymic: string,
    attendanceStatus: AttendanceStatus
    className: string,
}

export enum AttendanceStatus {
    Present = 0,
    Absent = 1
}

export interface User {
    name: string,
    surname: string,
    patronymic: string,
    email: string,
    username: string,
    phoneNumber: string,
}

export interface DayDate {
    day: string,
    date: string
}

export interface Auth {
    accessToken: string,
    email: string,
    groupId: string,
    roles: string[]
}