using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text.RegularExpressions;

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

        public async Task<ScheduleWrapper> GetGroupSchedule()
        {
            SortedSet<TimeSlot> timeSlots = new SortedSet<TimeSlot>();
            List<string> days = new List<string>();

            _webDriver.Navigate().GoToUrl($"http://fmi-schedule.chnu.edu.ua/schedule?semester=55&group=40");
            Thread.Sleep(1000);

            string groupName = _webDriver.FindElement(By.XPath("//*[@id=\"root\"]/div/section/section/div[1]/form/div[2]/div/div/div/input")).GetAttribute("value");
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
            var timeSlots = new SortedSet<TimeSlot>();

            var timeSlotElements = weekElement.FindElements(By.CssSelector(".MuiTableCell-root.MuiTableCell-body.lesson.groupLabelCell"));
            foreach (var timeSlotElement in timeSlotElements)
            {
                var timeSlotDetails = (timeSlotElement as WebElement).ComputedAccessibleLabel.Split();
                var timeSlot = new TimeSlot()
                {
                    Id = Int32.Parse(timeSlotDetails[0]),
                    ClassName = timeSlotDetails[0],
                    StartTime = TimeSpan.Parse(timeSlotDetails[1]),
                    EndTime = TimeSpan.Parse(timeSlotDetails[3]),
                };

                timeSlots.Add(timeSlot);
            }

            return timeSlots;
        }

        private List<string> GetDaysOfWeek(IWebElement weekElement)
        {
            var days = new List<string>();

            var dayElements = weekElement.FindElements(By.XPath("//thead/tr/th"));
            foreach (var day in dayElements)
            {
                if (!string.IsNullOrEmpty(day.Text) && !days.Contains(day.Text))
                {
                    days.Add(day.Text);
                }
            }

            return days;
        }

        private List<Schedule> ParseTableDataToSchedules(IWebElement weekElement, string groupName)
        {
            var resultSchedules = new List<Schedule>();

            var weekTable = weekElement.FindElements(By.XPath(".//tr"));
            foreach (var rows in weekTable)
            {
                var rowsData = rows.FindElements(By.XPath(".//td"));
                if (rowsData.Any())
                {
                    var timeSlotDetails = (rowsData.First() as WebElement).ComputedAccessibleLabel.Split();
                    var timeSlot = new TimeSlot()
                    {
                        Id = Int32.Parse(timeSlotDetails[0]),
                        ClassName = timeSlotDetails[0],
                        StartTime = TimeSpan.Parse(timeSlotDetails[1]),
                        EndTime = TimeSpan.Parse(timeSlotDetails[3]),
                    };

                    for (int i = 1; i < rowsData.Count; i++)
                    {
                        if (string.IsNullOrEmpty(rowsData[i].Text))
                        {
                            continue;
                        }

                        var schedule = ParseScheduleFromWebElement(rowsData[i], timeSlot);
                        schedule.Group = new Group() { Title = groupName };
                        resultSchedules.Add(schedule);
                    }
                }
            }

            return resultSchedules;
        }

        private Schedule ParseScheduleFromWebElement(IWebElement rowData, TimeSlot timeSlot)
        {
            var day = rowData.FindElement(By.XPath("./p")).GetAttribute("title");
            var cellData = rowData.Text.Split("\r\n");

            var teacherData = cellData[0].Split();
            var teacher = new Teacher()
            {
                Position = teacherData[0],
                Surname = teacherData[1],
                Name = teacherData[2],
                Patronymic = teacherData[3],
            };

            var classInfo = cellData[2].Substring(1, cellData[2].Length - 2).Split(',');
            var formatedClassInfo = classInfo.Select(p => p.Trim()).ToList();
            var @class = new Class()
            {
                Name = cellData[1],
                Teacher = teacher,
                LessonType = formatedClassInfo[0],
                Room = new Room() { Name = formatedClassInfo[1] },
            };
            var schedule = new Schedule()
            {
                TimeSlot = timeSlot,
                Class = @class,
                WeekType = "Odd",
                DayOfWeek = day,
            };

            return schedule;
        }
    }
}
