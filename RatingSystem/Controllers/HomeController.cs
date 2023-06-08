using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RatingSystem.Entities;
using RatingSystem.Services;
using RatingSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RatingSystem.Controllers
{
    public class HomeController : Controller
    {
        private AMSignInManager _signInManager;
        private AMUserManager _userManager;
        public AMSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<AMSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public AMUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AMUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private AMRolesManager _rolesManager;
        public AMRolesManager RolesManager
        {
            get
            {
                return _rolesManager ?? HttpContext.GetOwinContext().GetUserManager<AMRolesManager>();
            }
            private set
            {
                _rolesManager = value;
            }
        }
        public ActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            model.Employees = EmployeeServices.Instance.GetEmployee();
            return View(model);
        }


        [HttpGet]
        public ActionResult Rating(int ID, string BoxName, string BoxDesignation,string BoxImage)
        {
            RatingViewModel model = new RatingViewModel();
            model.EmployeeFull = EmployeeServices.Instance.GetEmployee(ID);
            ViewBag.NameBox = BoxName;
            ViewBag.DesignationBox = BoxDesignation;
            ViewBag.ImageBox = BoxImage;
            return View(model);
        }



        public ActionResult Thankyou()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Rating(RatingViewModel model)
        {
            var rating = new Rating();
            rating.CustomerService = model.CustomerService;
            rating.Professionalism = model.Professionalism;
            rating.Expertise = model.Expertise;
            rating.Respect = model.Respect;
            rating.Explanation = model.Explanation;
            rating.Treatment = model.Treatment;
            rating.Overall = model.Overall;
            rating.Employee = model.Employee;
            rating.TeamName = model.TeamName;

            rating.Date = DateTime.Now;
            RatingServices.Instance.SaveRating(rating);
            return Json(new { success = true },JsonRequestBehavior.AllowGet);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login(string Email, string Password)
        {
            if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password))
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            else
            {
                return View();
            }
        }


    }
}