using EFCoreCodeFirst.Entitites;
using EFCoreCodeFirst.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EFCoreCodeFirst.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }

    public IActionResult Index()
    {

      Student s = new Student();
      //s.Id = "1";

      StudentDepartment sd = new StudentDepartment(name:"Deneme");
      //sd.Id = "2";
      //sd.Name = "324432";

      return View();
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}