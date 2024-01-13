using Microsoft.AspNetCore.Mvc;
using Schedoo.Server.Services;

namespace Schedoo.Server.Controllers
{
    public class ScheduleController : Controller
    {
        [HttpGet("groupschedule")]
        public ActionResult GetGroups()
        {
            var scrapper = new ScrapperService();
            var res = scrapper.GetGroupSchedule();


            return Ok(res);
        }
    }
}
