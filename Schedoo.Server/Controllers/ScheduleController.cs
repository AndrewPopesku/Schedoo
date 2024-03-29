﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schedoo.Server.Data;
using Schedoo.Server.Helpers;
using Schedoo.Server.Models;
using Schedoo.Server.Services;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Identity;

namespace Schedoo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScheduleController(
        SchedooContext schedooContext, 
        ScrapperService scrapperService,
        UserManager<User> userManager)
        : ControllerBase
    {
        [HttpGet("groupschedule/groupName={groupName}")]
        public async Task<IActionResult> GetGroupSchedule(string groupName)
        {
            var (startWeekDate, endWeekDate) = Extensions.GetMondayAndFriday();

            var semester = await schedooContext.Semesters
                .FirstAsync(s => s.CurrentSemester);
            
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
                .Where(sd => sd.Schedule.Group.Name == groupName).ToList();

            if (!schedulesDb.Any())
            {
                await UpdateScheduleTable(groupName);
            }

            if (!scheduleDatesDb.Any())
            {
                await UpdateScheduleDateTable(schedulesDb, GetWeekType(semester.StartDay, DateTime.Now));
            }

            var result = new
            {
                scheduleAll = new
                {
                    oddWeekSchedule = schedulesDb.Where(cs => cs.WeekType == WeekType.Odd),
                    evenWeekSchedule = schedulesDb.Where(cs => cs.WeekType == WeekType.Even)
                },
                timeSlots = schedooContext.TimeSlots.ToImmutableSortedSet(),
                days = schedulesDb.Select(s => new
                {
                    day = s.DayOfWeek.ToString(),
                    date = Extensions.GetDateOfWeekDay(startWeekDate, s.DayOfWeek).ToString("dd/MM"),
                }).Distinct(),
                dates = scheduleDatesDb.Select(sd => new {Id = sd.Id, Date = sd.Date, ScheduleId = sd.ScheduleId})
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

            var currentSemester = await semestersDb.FirstAsync(s => s.CurrentSemester);
            var result = new
            {
                Semesters = semestersDb,
                WeekType = GetWeekType(currentSemester.StartDay, DateTime.Now),
            };

            return Ok(result);
        }

        private async Task UpdateScheduleDateTable(IEnumerable<Schedule> scheduleDb, WeekType weekType)
        {
            var scheduleDbIterable = scheduleDb
                .Where(s => s.WeekType == weekType);
            var scheduleDates = new List<ScheduleDate>();
            foreach (var s in scheduleDbIterable)
            {
                var newScheduleDate = new ScheduleDate
                {
                    ScheduleId = s.Id,
                    Schedule = s,
                    Date = Extensions.GetDateOfWeekDay(DateTime.Now, s.DayOfWeek)
                };
                
                scheduleDates.Add(newScheduleDate);
            }

            await schedooContext.ScheduleDates.AddRangeAsync(scheduleDates);
            await schedooContext.SaveChangesAsync();

            foreach (var sd in scheduleDates)
            {
                await UpdateAttendanceTable(sd.Id);
            }
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
        
        private async Task UpdateAttendanceTable(int scheduleDateId)
        {
            var scheduleDate = await schedooContext.ScheduleDates
                .Include(sd => sd.Schedule)
                .FirstAsync(sd => sd.Id == scheduleDateId);
            var roleName = "Student";
            var students = (await userManager.GetUsersInRoleAsync(roleName))
                .Where(s => s.GroupId == scheduleDate.Schedule.GroupId);
            
            foreach (var student in students)
            {
                var attendance = new Attendance
                {
                    ScheduleDateId = scheduleDate.Id,
                    StudentId = student.Id,
                    StudentFullName = student.Name + " " + student.SurName + " " + student.Patronymic,
                    AttendanceStatus = AttendanceStatus.Present,
                };
                
                await schedooContext.Attendances.AddAsync(attendance);
            }
    
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
        
        public static WeekType GetWeekType(DateTime termStartDate, DateTime dateToday)
        {
            var mondayDate = Extensions.GetDateOfWeekDay(termStartDate, DayOfWeek.Monday);
            var currentMondayDate = Extensions.GetDateOfWeekDay(dateToday, DayOfWeek.Monday);
            int count = 1;
            while (mondayDate.Date < currentMondayDate.Date)
            {
                mondayDate = mondayDate.AddDays(7);
                count++;
            }

            return count % 2 == 0 ? WeekType.Even : WeekType.Odd;
        }
    }
}
