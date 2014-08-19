using System.Web.Mvc;
using AspNet.Mvc.Grid.Example.Models;
using FizzWare.NBuilder;

namespace AspNet.Mvc.Grid.Example.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var people = Builder<Person>.CreateListOfSize(20)
                .All().With(p => p.Address = Builder<Address>.CreateNew().Build())
                .Build();

            return View(people);
        }
    }
}