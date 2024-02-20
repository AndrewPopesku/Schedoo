export const getSemestersReq = 'schedule/getsemesters';
export const getGroupsBySemesterIdReq = (selectedSemesterId: number) => 'schedule/getgroups/semesterId=' + selectedSemesterId;
export const getScheduleByGroupNameReq = (selectedGroupName: string) => 'schedule/groupschedule/groupName=' + selectedGroupName;
export const getAttendanceByIdReq = (attId: number) =>  'attendance/update/' + attId;
export const getAttendancesByScheduleDateIdReq = (scheduleDateId: string) => 'attendance/get/scheduleDateId=' + scheduleDateId;