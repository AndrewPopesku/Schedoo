using System.Globalization;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Schedoo.Server.Helpers;
using Schedoo.Server.Models;
using Group = Schedoo.Server.Models.Group;

namespace Schedoo.Server.Services
{
    public class ScrapperService
    {
        public IEnumerable<Schedule> GetGroupSchedule(string groupNameSelected)
        {
            ChromeOptions options = new ChromeOptions();
            // options.AddArgument("--headless");
            List<Schedule> oddWeekClasses;
            List<Schedule>  evenWeekClasses;
            var webDriver = new ChromeDriver(options);
            
            webDriver.Navigate().GoToUrl($"http://fmi-schedule.chnu.edu.ua/schedule");
            Thread.Sleep(2000);
            
            webDriver.FindElement(By.XPath("/html/body/div[1]/div/section/section/div[1]/form/div[2]/div/div/div/input"))
                .SendKeys(groupNameSelected);
            
            webDriver.FindElement(By.XPath("/html/body/div[4]/div/ul/li")).Click();
            // Thread.Sleep(1000);
            webDriver.FindElement(By.CssSelector(
                "#root > div > section > section > div.card.form-card.schedule-form-card > form > button")).Click();

            var groupName = webDriver
                .FindElement(By.XPath("//*[@id=\"root\"]/div/section/section/div[1]/form/div[2]/div/div/div/input"))
                .GetAttribute("value");
            Thread.Sleep(2000);
            var weekTables = webDriver.FindElements(By.ClassName("MuiTable-root"));
            Thread.Sleep(2000);
            oddWeekClasses = ParseTableDataToSchedules(weekTables[0], "odd", groupName);
            evenWeekClasses = ParseTableDataToSchedules(weekTables[1], "even", groupName);
            
            webDriver.Quit();
            return oddWeekClasses.Concat(evenWeekClasses);
        }

        public List<Group> GetGroups(string semesterName)
        {
            var resGroups = new List<Group>();
            
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            var webDriver = new ChromeDriver(options);
            webDriver.Navigate().GoToUrl("http://fmi-schedule.chnu.edu.ua/schedule?semester=55");
            Thread.Sleep(2000);

            webDriver.FindElement(By.CssSelector(".MuiAutocomplete-root:nth-child(2) .MuiButtonBase-root:nth-child(2) .MuiSvgIcon-root"))
                .Click();
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
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
                resGroups = groupWebElements.Select(g => new Group(g.Text)).ToList();
            }

            return resGroups;
        }

        public List<Semester> GetSemesters()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            var webDriver = new ChromeDriver(options);
            
            var resSemesters = new List<Semester>();

            webDriver.Navigate().GoToUrl("http://fmi-schedule.chnu.edu.ua/");
            Thread.Sleep(2000);

            webDriver.FindElement(By.CssSelector(".MuiAutocomplete-root:nth-child(1) .MuiButtonBase-root:nth-child(2) .MuiSvgIcon-root")).Click();

            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
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
                var semesterDates = webDriver.FindElement(By.XPath("/html/body/div[1]/div/h1")).Text;
                
                string pattern = @"\b(\d{2})/(\d{2})/(\d{4})\b";
                MatchCollection matches = Regex.Matches(semesterDates, pattern);
        
                string startDateString = matches[0].Value;
                string endDateString = matches[1].Value;
        
                DateTime startDate = DateTime.ParseExact(startDateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(endDateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                resSemesters = semesterWebElements.Select(s => new Semester
                {
                    Description = s.Text,
                    StartDay = startDate,
                    EndDay = endDate
                }).ToList();
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

            var group = new Group(groupName);
            var schedule = new Schedule
            {
                ClassId = @class.Id,
                TimeSlotId = timeSlot.Id,
                GroupId = group.Id,
                TimeSlot = timeSlot,
                Class = @class,
                Group = group,
                WeekType = weekType.ToWeekType(),
                DayOfWeek = day.ToDayOfWeek(),
            };

            return schedule;
        }
    }
}
