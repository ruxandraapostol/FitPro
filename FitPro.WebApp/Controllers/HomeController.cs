using FitPro.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FitPro.WebApp.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController(ControllerDependencies dependencies, ILogger<HomeController> logger) : base(dependencies)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("/NotFound")]
        public IActionResult Error_NotFound()
        {
            return View();
        }

        [Route("/InternalProblem")]
        public IActionResult Error_InternalServerError()
        {
            return View();
        }

        [Route("/Anauthorized")]
        public IActionResult Error_Unauthorized()
        {
            return View();
        }
    }
}
