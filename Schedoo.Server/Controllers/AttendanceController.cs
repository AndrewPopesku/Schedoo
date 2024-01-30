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
        [Authorize(Roles = "Administrator,Group Leader")]
        [HttpGet("get")]
        public async Task<IActionResult> GetAttendances()
        {
            var (startWeekDay, endWeekDay) = Extensions.GetMondayAndFriday();
            var attendances = schedooContext.Attendances
                .Include(a => a.ScheduleDate)
                .Include(a => a.Student);
            
            if (!attendances.Any(a => a.ScheduleDate.Date >= startWeekDay && a.ScheduleDate.Date <= endWeekDay))
            {
                var scheduleDates = schedooContext.ScheduleDates
                    .Where(sd => sd.Date >= startWeekDay && sd.Date <= endWeekDay);
                var roleName = "Student";
                var students = await userManager.GetUsersInRoleAsync(roleName);
                
                foreach (var sd in scheduleDates)
                {
                    foreach (var student in students)
                    {
                        var attendance = new Attendance
                        {
                            ScheduleDate = sd,
                            Student = student,
                            AttendanceStatus = AttendanceStatus.Present,
                        };

                        await schedooContext.Attendances.AddAsync(attendance);
                    }
                }

                await schedooContext.SaveChangesAsync();
            }

            return Ok(attendances);
        }
    }
}
