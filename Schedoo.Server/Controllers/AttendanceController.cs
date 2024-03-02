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
        [HttpGet("get/scheduleDateId={scheduleDateId}")]
        public async Task<IActionResult> GetAttendances(int scheduleDateId)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var groupUser = await schedooContext.Groups.FindAsync(user.GroupId);
            
            var (startWeekDay, endWeekDay) = Extensions.GetMondayAndFriday();
            var attendances = schedooContext.Attendances
                .Where(a => a.ScheduleDate.Id == scheduleDateId)
                .Include(a => a.ScheduleDate)
                    .ThenInclude(sd => sd.Schedule)
                        .ThenInclude(s => s.Class);

            var groupAttendance =
                await schedooContext.Groups.FindAsync(attendances.FirstOrDefault().ScheduleDate.Schedule.GroupId);
            if (groupUser.Name.Substring(0, 3) ==
                groupAttendance.Name.Substring(0, 3))
            {
                var result = attendances.Select(a => new
                {
                    Id = a.Id,
                    AttendanceStatus = a.AttendanceStatus,
                    ClassName = a.ScheduleDate.Schedule.Class.Name,
                    ScheduleDateId = a.ScheduleDateId,
                    StudentFullName = a.StudentFullName,
                    Date = a.ScheduleDate.Date.ToString("dd/MM")
                });
                
                return Ok(result);
            }

            return Forbid();
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
        
        
    }
}
