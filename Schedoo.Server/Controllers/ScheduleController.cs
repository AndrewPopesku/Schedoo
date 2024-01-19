using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schedoo.Server.Data;
using Schedoo.Server.Helpers;
using Schedoo.Server.Models;
using Schedoo.Server.Services;
using System.Collections.Immutable;

namespace Schedoo.Server.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly SchedooContext _schedooContext;
        private readonly ScrapperService _scrapperService;
        public ScheduleController(SchedooContext schedooContext, ScrapperService scrapperService)
        {
            _schedooContext = schedooContext;
            _scrapperService = scrapperService;
        }

        [HttpGet("groupschedule")]
        public async Task<ActionResult> GetGroupSchedule()
        {
            var scheduleDb = _schedooContext.Schedules
                             .Include(s => s.TimeSlot)
                             .Include(s => s.Class);

            if (!scheduleDb.Any())
            {
                var scheduleScrapped = _scrapperService.GetGroupSchedule();
                var timeslots = scheduleScrapped.Select(s => s.TimeSlot).Distinct();
                var classes = scheduleScrapped.Select(s => s.Class).Distinct();
                var scheduleToDb = scheduleScrapped.Select(s => new Schedule(s));

                await _schedooContext.AddRangeAsync(classes);
                await _schedooContext.AddRangeAsync(timeslots);
                await _schedooContext.AddRangeAsync(scheduleToDb);
                await _schedooContext.SaveChangesAsync();
            }

            var result = new ScheduleWrapper()
            {
                ScheduleAll = new ScheduleWrapper.ScheduleCat(
                    scheduleDb.Where(s => s.WeekType == "odd"),
                    scheduleDb.Where(s => s.WeekType == "even")
                ),
                TimeSlots = _schedooContext.TimeSlots.ToImmutableSortedSet(),
                Days = scheduleDb.Select(s => s.DayOfWeek).Distinct(),
            };

            return Ok(result);
        }

        [HttpGet("getgroups")]
        public ActionResult GetGroups()
        {
            return Ok(_scrapperService.GetGroups());
        }

        [HttpGet("getsemesters")]
        public ActionResult GetSemesters()
        {
            var semesters = _scrapperService.GetSemesters();

            return Ok(semesters);
        }
    }
}
