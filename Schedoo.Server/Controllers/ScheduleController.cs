using Microsoft.AspNetCore.Mvc;
using Schedoo.Server.Services;

namespace Schedoo.Server.Controllers
{
    public class ScheduleController : Controller
    {
        private ScrapperService scrapperService;
        public ScheduleController()
        {
            scrapperService = new ScrapperService();
        }

        [HttpGet("groupschedule")]
        public ActionResult GetGroupSchedule()
        {
            var res = scrapperService.GetGroupSchedule();

            return Ok(res);
        }

        [HttpGet("getgroups")]
        public ActionResult GetGroups()
        {
            var groups = scrapperService.GetGroups();

            return Ok(groups);
        }

        [HttpGet("getsemesters")]
        public ActionResult GetSemesters()
        {
            var semesters = scrapperService.GetSemesters();

            return Ok(semesters);
        }
    }
}
