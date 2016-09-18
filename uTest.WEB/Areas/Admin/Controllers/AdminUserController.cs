using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using uTest.Auth.BLL.DTO;
using uTest.Auth.BLL.Interfaces;
using uTest.BLL.Interfaces;
using uTest.WEB.Util;
using uTest.WEB.ViewModels;

namespace uTest.WEB.Areas.Admin.Controllers
{
    public class AdminUserController : Controller
    {
        private IUserService UserService =>
            HttpContext.GetOwinContext().GetUserManager<IUserService>();

        readonly ITestService _testService;
        public AdminUserController(ITestService serv)
        {
            _testService = serv;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserViewModel>());
            var mapper = config.CreateMapper();
            return View(mapper.Map<IEnumerable<UserViewModel>>(UserService.GetAll()));
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            var mapper = MapperConfig.GetConfigToViewModel().CreateMapper();
            var userView = mapper.Map<UserViewModel>(UserService.Get(id));
            userView.SolvedTests = mapper.Map<IEnumerable<SolvedTestViewModel>>(_testService.GetSolvedTests(id)).ToList();
            ViewBag.Stat = _testService.GetStatistic(id);
            return View(userView);
        }
    }
}