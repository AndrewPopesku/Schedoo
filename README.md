# Schedoo (Student Schedule Management System)

## Overview
This project is a Student Schedule Management System designed to help students and administrators manage and view schedules efficiently. The system allows users to log in, view their schedules, and track attendance. It includes different roles such as student, group leader, and admin, each with varying levels of access and capabilities.

## Table of Contents
1. [Main Page](#main-page)
2. [Login Page](#login-page)
3. [Student Dashboard](#student-dashboard)
4. [Odd Week Schedule](#odd-week-schedule)
5. [Profile Page](#profile-page)
6. [Group Leader Dashboard](#group-leader-dashboard)
7. [Attendance Journal](#attendance-journal)
8. [Attendance Statistics](#attendance-statistics)

## Main Page
The main page displays various schedules that can be selected through query string parameters or on-page controls.
![Main Page](https://github.com/AndrewPopesku/Schedoo/assets/101664066/dfd4d86a-e5c5-4a95-b770-858397334f8d)

## Login Page
To access personalized schedules, click the "LOG IN" button, which will redirect you to the login form. Enter your login credentials. The form will alert you if the credentials fail validation.
![Login Page](https://github.com/AndrewPopesku/Schedoo/assets/101664066/884ee5ca-1ea7-4a5d-9996-b6671bb3edac)

## Student Dashboard
After logging in, students will see their personal schedule. A blue square indicates the current class, and a blue circle next to the day of the week indicates the current day.
![Student Dashboard](https://github.com/AndrewPopesku/Schedoo/assets/101664066/5c271371-78f2-4698-afce-3739d29c497c)

## Odd Week Schedule
Clicking on "Odd" will switch to the odd week schedule, displaying only the schedule for that week without any highlights.
![Odd Week Schedule](images/odd_week_schedule.png)

## Profile Page
In the profile section, students can view the number of classes attended in the past week. Due to limited permissions, regular students can only see their own data.
![Profile Page](https://github.com/AndrewPopesku/Schedoo/assets/101664066/27904e80-c862-45c2-a4e9-b3239ad07bbe)

## Group Leader Dashboard
Group leaders have additional buttons within the schedule grid that navigate to the attendance journal editing page for specific classes.
![Group Leader Dashboard](https://github.com/AndrewPopesku/Schedoo/assets/101664066/19b56d72-ce5d-49e8-98fb-e53d841dcc75)

## Attendance Journal
Group leaders can edit the attendance records for their respective groups. Changes will be reflected for all users and in the overall statistics.
![Attendance Journal](images/attendance_journal.png)

## Attendance Statistics
Group leaders can view and export attendance statistics for the last month, categorized by class type (lectures, practicals, labs).
![Attendance Statistics](https://github.com/AndrewPopesku/Schedoo/assets/101664066/926d32d7-caba-4dde-8b55-87fcd3b53189)

![image](https://github.com/AndrewPopesku/Schedoo/assets/101664066/64c7b7ac-2e8c-4e6c-8aba-068e868cb6e7)

![Exported Statistics](https://github.com/AndrewPopesku/Schedoo/assets/101664066/b5f72806-1ebe-4b9f-a248-de739185e47e)

