using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Schedoo.Server.Helpers;
using Schedoo.Server.Models;
using System.Globalization;

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

        public IEnumerable<Schedule> GetGroupSchedule()
        {
            _webDriver.Navigate().GoToUrl($"http://fmi-schedule.chnu.edu.ua/schedule?semester=55&group=40");
            Thread.Sleep(1000);

            var groupName = _webDriver.FindElement(By.XPath("//*[@id=\"root\"]/div/section/section/div[1]/form/div[2]/div/div/div/input")).GetAttribute("value");
            var weekTables = _webDriver.FindElements(By.ClassName("MuiTable-root"));
            var oddWeekClasses = ParseTableDataToSchedules(weekTables[0], "odd", groupName);
            var evenWeekClasses = ParseTableDataToSchedules(weekTables[1], "even", groupName);

            return oddWeekClasses.Concat(evenWeekClasses);
        }

        public List<string> GetGroups()
        {
            var resGroups = new List<string>();

            _webDriver.Navigate().GoToUrl("http://fmi-schedule.chnu.edu.ua/schedule?semester=55");
            Thread.Sleep(2000);

            _webDriver.FindElement(By.CssSelector(".MuiAutocomplete-root:nth-child(2) .MuiButtonBase-root:nth-child(2) .MuiSvgIcon-root"))
                .Click();
            WebDriverWait wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
            IWebElement? autocompleteDropdown = wait.Until(d =>
            {
                try
                {
                    IWebElement element = d.FindElement(By.XPath("/html/body/div[4]/div/ul"));
                    return element.Displayed ? element : null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            });
            
            Thread.Sleep(2000);
            if (autocompleteDropdown != null)
            {
                var groupWebElements = autocompleteDropdown.FindElements(By.CssSelector(".MuiAutocomplete-option"));
                resGroups = groupWebElements.Select(g => g.Text).ToList();
            }

            return resGroups;
        }

        public List<Semester> GetSemesters()
        {
            var resSemesters = new List<Semester>();

            _webDriver.Navigate().GoToUrl("http://fmi-schedule.chnu.edu.ua/");
            Thread.Sleep(2000);

            _webDriver.FindElement(By.CssSelector(".MuiAutocomplete-root:nth-child(1) .MuiButtonBase-root:nth-child(2) .MuiSvgIcon-root")).Click();

            WebDriverWait wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
            IWebElement? autocompleteDropdown = wait.Until(d =>
            {
                try
                {
                    IWebElement element = d.FindElement(By.XPath("/html/body/div[4]/div/ul"));
                    return element.Displayed ? element : null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            });

            Thread.Sleep(2000);
            if (autocompleteDropdown != null)
            {
                var semesterWebElements = autocompleteDropdown.FindElements(By.CssSelector(".MuiAutocomplete-option"));
                resSemesters = semesterWebElements.Select(s => new Semester { Description = s.Text }).ToList();
            }

            return resSemesters;
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

        private List<Schedule> ParseTableDataToSchedules(IWebElement weekElement, string weekType, string groupName)
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
                        StartTime = TimeSpan.Parse(timeSlotDetails[1]),
                        EndTime = TimeSpan.Parse(timeSlotDetails[3]),
                    };

                    resultSchedules.AddRange(
                        rowData.Skip(1)
                               .Where(d => !string.IsNullOrEmpty(d.Text))
                               .Select(d => ParseScheduleFromWebElement(d, timeSlot, weekType, groupName))
                    );
                }
            }

            return resultSchedules;
        }

        private Schedule ParseScheduleFromWebElement(IWebElement rowData, TimeSlot timeSlot, string weekType, string groupName)
        {
            var day = rowData.FindElement(By.XPath("./p")).GetAttribute("title");
            var linkToMeeting = rowData.FindElement(By.XPath("//title")).Text;

            var cellData = rowData.Text.Split("\r\n");

            var teacherData = cellData[0].Split();

            var teacherId = Guid.NewGuid();
            var teacher = new Teacher
            {
                Id = teacherId,
                Position = teacherData[0],
                Surname = teacherData[1],
                Name = teacherData[2],
                Patronymic = teacherData[3],
            };

            var classInfo = cellData[2].Substring(1, cellData[2].Length - 2)
                                       .Split(',')
                                       .Select(p => p.Trim())
                                       .ToList();

            var roomId = Guid.NewGuid();
            var @class = new Class
            {
                Id = Guid.NewGuid(),
                Name = cellData[1],
                Teacher = teacher,
                TeacherId = teacherId,
                LessonType = classInfo[0],
                LinkToMeeting = linkToMeeting,
                Room = new Room { Id = roomId, Name = classInfo[1] },
                RoomId = roomId,
            };

            var schedule = new Schedule
            {
                ClassId = @class.Id,
                TimeSlotId = timeSlot.Id,
                TimeSlot = timeSlot,
                Class = @class,
                WeekType = weekType,
                DayOfWeek = day,
                GroupId = groupName,
            };

            return schedule;
        }
    }
}
