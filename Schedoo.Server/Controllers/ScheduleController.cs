using Microsoft.AspNetCore.Mvc;

namespace Schedoo.Server.Controllers
{
    public class ScheduleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
