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
        private static object Lock = new();
        
        // [Authorize(Roles = "Administrator,Group Leader")]
        [HttpGet("get/scheduleDateId={scheduleDateId}")]
        public async Task<IActionResult> GetAttendances(int scheduleDateId)
        {
            var (startWeekDay, endWeekDay) = Extensions.GetMondayAndFriday();
            var attendances = schedooContext.Attendances
                .Where(a => a.ScheduleDate.Id == scheduleDateId)
                .Include(a => a.ScheduleDate)
                    .ThenInclude(sd => sd.Schedule)
                        .ThenInclude(s => s.Class)
                .Include(a => a.Student);
            
            if (!attendances.Any(a => a.ScheduleDate.Date >= startWeekDay
                                      && a.ScheduleDate.Date <= endWeekDay))
            {
                await UpdateAttendanceTable(startWeekDay, endWeekDay, scheduleDateId);
            }

            var result = attendances.Select(a => new
            {
                Id = a.Id,
                AttendanceStatus = a.AttendanceStatus,
                ClassName = a.ScheduleDate.Schedule.Class.Name,
                ScheduleDateId = a.ScheduleDateId,
                StudentName = a.Student.Name,
                StudentSurname = a.Student.SurName,
                StudentPatronymic = a.Student.Patronymic,
                Date = a.ScheduleDate.Date
            });
            
            return Ok(result);
        }
        
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateAttendance(int id, [FromBody] AttendanceStatus attendanceStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entityToUpdate = await schedooContext.Attendances
                .FirstAsync(a => a.Id == id);

            if (entityToUpdate == null)
            {
                return NotFound();
            }

            entityToUpdate.AttendanceStatus = attendanceStatus;
            
            schedooContext.Entry(entityToUpdate).State = EntityState.Modified;
            await schedooContext.SaveChangesAsync();

            return NoContent();
        }
        
        private async Task UpdateAttendanceTable(DateTime startWeekDay, DateTime endWeekDay, int scheduleDateId)
        {
            var attendanceExists = await schedooContext.Attendances
                .AnyAsync(a => a.ScheduleDateId == scheduleDateId);
            
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
