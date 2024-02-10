using Microsoft.EntityFrameworkCore;
using Schedoo.Server.Models;

namespace Schedoo.Server.Data
{
    public class SchedooContext(DbContextOptions<SchedooContext> options) : DbContext(options)
    {
        public DbSet<Class> Classes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleDate> ScheduleDates { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
    }
}
