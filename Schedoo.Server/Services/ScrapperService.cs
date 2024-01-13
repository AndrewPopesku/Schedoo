using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Schedoo.Server.Helpers;
using Schedoo.Server.Models;
using System.Globalization;
using MGroup = Schedoo.Server.Models.Group;

namespace Schedoo.Server.Services
{
    public class ScrapperService
    {
        private readonly IWebDriver _webDriver;
        public ScrapperService()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            _webDriver = new ChromeDriver(options);
        }

        public ScheduleWrapper GetGroupSchedule()
        {
            _webDriver.Navigate().GoToUrl($"http://fmi-schedule.chnu.edu.ua/schedule?semester=55&group=40");
            Thread.Sleep(1000);

            var groupName = _webDriver.FindElement(By.XPath("//*[@id=\"root\"]/div/section/section/div[1]/form/div[2]/div/div/div/input")).GetAttribute("value");
            var weekTables = _webDriver.FindElements(By.ClassName("MuiTable-root"));
            var oddWeekClasses = ParseTableDataToSchedules(weekTables[0], groupName);
            var evenWeekClasses = ParseTableDataToSchedules(weekTables[1], groupName);

            return new ScheduleWrapper()
            {
                ScheduleAll = new ScheduleWrapper.ScheduleCat(oddWeekClasses, evenWeekClasses),
                TimeSlots = GetTimeSlots(weekTables[0]),
                Days = GetDaysOfWeek(weekTables[0]),
            };
        }

        private SortedSet<TimeSlot> GetTimeSlots(IWebElement weekElement)
        {
            var resultingTimeSlots = new SortedSet<TimeSlot>();

            var timeSlotElements = weekElement.FindElements(By.CssSelector(".MuiTableCell-root.MuiTableCell-body.lesson.groupLabelCell"));
            foreach (var timeSlotElement in timeSlotElements)
            {
                if (timeSlotElement is WebElement timeSlotWebElement)
                {
                    var timeSlotDetails = timeSlotWebElement.ComputedAccessibleLabel.Split();
                    var timeSlot = new TimeSlot
                    {
                        Id = int.Parse(timeSlotDetails[0]),
                        ClassName = timeSlotDetails[0],
                        StartTime = TimeSpan.Parse(timeSlotDetails[1], CultureInfo.InvariantCulture),
                        EndTime = TimeSpan.Parse(timeSlotDetails[3], CultureInfo.InvariantCulture),
                    };

                    resultingTimeSlots.Add(timeSlot);
                }

            }

            return resultingTimeSlots;
        }

        private List<string> GetDaysOfWeek(IWebElement weekElement)
        {
            var dayElements = weekElement.FindElements(By.XPath("//thead/tr/th"));

            var days = dayElements.Where(day => !string.IsNullOrEmpty(day.Text))
                                  .Select(day => day.Text)
                                  .Distinct()
                                  .ToList();

            return days;
        }

        private List<Schedule> ParseTableDataToSchedules(IWebElement weekElement, string groupName)
        {
            var resultSchedules = new List<Schedule>();

            var weekTable = weekElement.FindElements(By.XPath(".//tr"));
            foreach (var rows in weekTable)
            {
                var rowData = rows.FindElements(By.XPath(".//td"));

                if (rowData.Any() && rowData[0] is WebElement rowDataWebEl)
                {
                    var timeSlotDetails = rowDataWebEl.ComputedAccessibleLabel.Split();
                    var timeSlot = new TimeSlot()
                    {
                        Id = Int32.Parse(timeSlotDetails[0]),
                        ClassName = timeSlotDetails[0],
                        StartTime = TimeSpan.Parse(timeSlotDetails[1], CultureInfo.InvariantCulture),
                        EndTime = TimeSpan.Parse(timeSlotDetails[3], CultureInfo.InvariantCulture),
                    };

                    resultSchedules.AddRange(
                        rowData.Where(d => !string.IsNullOrEmpty(d.Text))
                            .Select(d => ParseScheduleFromWebElement(d, timeSlot, groupName))
                    );
                }
            }

            return resultSchedules;
        }

        private Schedule ParseScheduleFromWebElement(IWebElement rowData, TimeSlot timeSlot, string groupName)
        {
            var day = rowData.FindElement(By.XPath("./p")).GetAttribute("title");
            var cellData = rowData.Text.Split("\r\n");

            var teacherData = cellData[0].Split();
            var teacher = new Teacher
            {
                Position = teacherData[0],
                Surname = teacherData[1],
                Name = teacherData[2],
                Patronymic = teacherData[3],
            };

            var classInfo = cellData[2].Substring(1, cellData[2].Length - 2)
                                       .Split(',')
                                       .Select(p => p.Trim())
                                       .ToList();

            var @class = new Class
            {
                Name = cellData[1],
                Teacher = teacher,
                LessonType = classInfo[0],
                Room = new Room { Name = classInfo[1] },
            };

            var schedule = new Schedule
            {
                TimeSlot = timeSlot,
                Class = @class,
                WeekType = "Odd",
                DayOfWeek = day,
                Group = new MGroup { Title = groupName },
            };

            return schedule;
        }

    }
}
