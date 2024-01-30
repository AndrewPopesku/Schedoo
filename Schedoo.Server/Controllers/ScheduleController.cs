using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schedoo.Server.Data;
using Schedoo.Server.Helpers;
using Schedoo.Server.Models;
using Schedoo.Server.Services;
using System.Collections.Immutable;

namespace Schedoo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScheduleController(SchedooContext schedooContext, ScrapperService scrapperService)
        : ControllerBase
    {
        [HttpGet("groupschedule")]
        public async Task<IActionResult> GetGroupSchedule()
        {
            var scheduleDb = schedooContext.Schedules
                             .Include(s => s.TimeSlot)
                             .Include(s => s.Class);
            var (startWeekDate, endWeekDate) = Extensions.GetMondayAndFriday();

            if (!scheduleDb.Any())
            {
                var scheduleScrapped = scrapperService.GetGroupSchedule();
                var timeslots = scheduleScrapped.Select(s => s.TimeSlot).Distinct();
                var classes = scheduleScrapped.Select(s => s.Class).Distinct();
                var scheduleToDb = scheduleScrapped.Select(s => new Schedule(s));

                await schedooContext.AddRangeAsync(classes);
                await schedooContext.AddRangeAsync(timeslots);
                await schedooContext.AddRangeAsync(scheduleToDb);
                await schedooContext.SaveChangesAsync();
            }

            if (!schedooContext.ScheduleDates
                .Any(sd => sd.Date >= startWeekDate && sd.Date <= endWeekDate))
            {
                var scheduleDbIterable = scheduleDb.Where(s => s.WeekType == "odd");
                foreach (var s in scheduleDbIterable)
                {
                    var newScheduleDate = new ScheduleDate
                    {
                        ScheduleId = s.Id,
                        Date = Extensions.GetDateOfWeekDay(s.DayOfWeek)
                    };

                    schedooContext.ScheduleDates.Add(newScheduleDate);
                }

                await schedooContext.SaveChangesAsync();
            }

            var result = new ScheduleWrapper()
            {
                ScheduleAll = new ScheduleWrapper.ScheduleCat(
                    scheduleDb.Where(s => s.WeekType == "odd"),
                    scheduleDb.Where(s => s.WeekType == "even")
                ),
                TimeSlots = schedooContext.TimeSlots.ToImmutableSortedSet(),
                Days = scheduleDb.Select(s => s.DayOfWeek).Distinct(),
            };

            return Ok(result);
        }

        
        [HttpGet("getgroups")]
        public IActionResult GetGroups()
        {
            return Ok(scrapperService.GetGroups());
        }

        [HttpGet("getsemesters")]
        public IActionResult GetSemesters()
        {
            var semesters = scrapperService.GetSemesters();

            return Ok(semesters);
        }
    }
}
