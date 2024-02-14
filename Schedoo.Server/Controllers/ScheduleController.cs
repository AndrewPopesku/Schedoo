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
        [HttpGet("groupschedule/groupName={groupName}")]
        public async Task<IActionResult> GetGroupSchedule(string groupName)
        {
            var (startWeekDate, endWeekDate) = Extensions.GetMondayAndFriday();

            var schedulesDb = schedooContext.Schedules
                .Include(s => s.Group)
                .Where(s => s.Group.Name == groupName)
                .Include(s => s.TimeSlot)
                .Include(s => s.Class)
                    .ThenInclude(c => c.Teacher)
                .Include(s => s.Class)
                    .ThenInclude(c => c.Room);

            var scheduleDatesDb = schedooContext.ScheduleDates
                .Where(sd => sd.Date >= startWeekDate && sd.Date <= endWeekDate)
                .Include(sd => sd.Schedule).ThenInclude(s => s.Group)
                .Where(sd => sd.Schedule.Group.Name == groupName);

            if (!schedulesDb.Any())
            {
                await UpdateScheduleTable(groupName);
            }

            if (!scheduleDatesDb.Any())
            {
                await UpdateScheduleDateTable(schedulesDb, WeekType.Odd);
                await UpdateScheduleDateTable(schedulesDb, WeekType.Even);
            }

            var result = new
            {
                ScheduleAll = new
                {
                    OddWeekSchedule = schedulesDb.Where(cs => cs.WeekType == WeekType.Odd),
                    EvenWeekSchedule = schedulesDb.Where(cs => cs.WeekType == WeekType.Even)
                },
                TimeSlots = schedooContext.TimeSlots.ToImmutableSortedSet(),
                Days = schedulesDb.Select(s => s.DayOfWeek.ToString()).Distinct(),
                Dates = scheduleDatesDb.Select(sd => new {Id = sd.Id, Date = sd.Date, ScheduleId = sd.ScheduleId})
            };

            return Ok(result);
        }

        [HttpGet("getgroups/semesterId={semesterId}")]
        public async Task<IActionResult> GetGroups(int semesterId)
        {
            // var groupsDb = schedooContext.Groups;
            var semester = await schedooContext.Semesters
                .FirstAsync(s => s.Id == semesterId);
            var semesterName = semester.Description;
            // var groupsDb = scrapperService.GetGroups(semesterName);
            var groupsDb = schedooContext.Groups;
            if (!groupsDb.Any())
            {
                var groupsScrapped = scrapperService.GetGroups(semesterName);
                await schedooContext.AddRangeAsync(groupsScrapped);
                await schedooContext.SaveChangesAsync();
            }
            
            return Ok(groupsDb.OrderBy(g => g.Name));
        }

        [HttpGet("getsemesters")]
        public async Task<IActionResult> GetSemesters()
        {
            var semestersDb = schedooContext.Semesters;

            if (!semestersDb.Any())
            {
                var semestersScrapped = scrapperService.GetSemesters();
                schedooContext.UpdateRange(semestersScrapped);
                await schedooContext.SaveChangesAsync();
            }
            
            return Ok(semestersDb);
        }

        private async Task UpdateScheduleDateTable(IEnumerable<Schedule> scheduleDb, WeekType weekType)
        {
            var scheduleDbIterable = scheduleDb
                .Where(s => s.WeekType == weekType);
            foreach (var s in scheduleDbIterable)
            {
                var newScheduleDate = new ScheduleDate
                {
                    ScheduleId = s.Id,
                    Schedule = s,
                    Date = Extensions.GetDateOfWeekDay(s.DayOfWeek)
                };

                await schedooContext.ScheduleDates.AddAsync(newScheduleDate);
            }

            await schedooContext.SaveChangesAsync();
        }

        private async Task UpdateScheduleTable(string groupName)
        {
            var scheduleScrapped = scrapperService.GetGroupSchedule(groupName);
            var timeslots = scheduleScrapped.Select(s => s.TimeSlot).Distinct();
            var classes = scheduleScrapped.Select(s => s.Class).Distinct();
            var teachers = scheduleScrapped.Select(s => s.Class.Teacher).Distinct();
            var rooms = scheduleScrapped.Select(s => s.Class.Room).Distinct();
            var scheduleToDb = scheduleScrapped.Select(s => new Schedule(s));
            
            await AddRangeIfNotExistAsync(classes);
            await AddRangeIfNotExistAsync(timeslots);
            await AddRangeIfNotExistAsync(teachers);
            await AddRangeIfNotExistAsync(rooms);
            await AddRangeIfNotExistAsync(scheduleToDb);
            await schedooContext.SaveChangesAsync();
        }

        private async Task AddRangeIfNotExistAsync<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)  
            {
                if (!schedooContext.Set<T>().Local.Any(entry => entry.Equals(entity))
                    && !await schedooContext.Set<T>().AnyAsync(entry => entry.Equals(entity)))
                {
                    schedooContext.Set<T>().Add(entity);
                }
            }
        }
    }
}
