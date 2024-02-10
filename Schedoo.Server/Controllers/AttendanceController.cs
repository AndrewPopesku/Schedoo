using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schedoo.Server.Data;
using Schedoo.Server.Helpers;
using Schedoo.Server.Models;

namespace Schedoo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AttendanceController(SchedooContext schedooContext, UserManager<User> userManager)
        : ControllerBase
    {
        // [Authorize(Roles = "Administrator,Group Leader")]
        [HttpGet("get/{scheduleDateId}")]
        public async Task<IActionResult> GetAttendances(int scheduleDateId)
        {
            var (startWeekDay, endWeekDay) = Extensions.GetMondayAndFriday();
            var attendances = schedooContext.Attendances
                .Where(a => a.ScheduleDate.Id == scheduleDateId)
                .Include(a => a.ScheduleDate)
                .Include(a => a.Student);
            
            if (!attendances.Any(a => a.ScheduleDate.Date >= startWeekDay 
                                      && a.ScheduleDate.Date <= endWeekDay))
            {
                await UpdateAttendanceTable(startWeekDay, endWeekDay, scheduleDateId);
            }

            return Ok(attendances);
        }
        
        private async Task UpdateAttendanceTable(DateTime startWeekDay, DateTime endWeekDay, int scheduleDateId) {
            var scheduleDate = await schedooContext.ScheduleDates
                .FirstAsync(sd => sd.Id == scheduleDateId);
            var roleName = "Student";
            var students = await userManager.GetUsersInRoleAsync(roleName);
                
            foreach (var student in students)
            {
                var attendance = new Attendance
                {
                    ScheduleDateId = scheduleDate.Id,
                    StudentId = student.Id,
                    AttendanceStatus = AttendanceStatus.Present,
                };

                await schedooContext.Attendances.AddAsync(attendance);
            }

            await schedooContext.SaveChangesAsync();
        }
    }
}
