using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RatingSystem.Services;
using RatingSystem.ViewModels;
using RatingSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using System.Web.WebPages;

namespace RatingSystem.Controllers
{
    public class AdminController : Controller
    {
        private AMSignInManager _signInManager;
        private AMRolesManager _rolesManager;
        private AMUserManager _userManager;
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
        // GET: Admin
        public ActionResult Index()
        {
            AdminViewModel model = new AdminViewModel();
            var user = UserManager.FindById(User.Identity.GetUserId());
            model.Name = user.Name;
            return View(model);
        }

        [HttpGet]
        public ActionResult Dashboard()
        {
            AdminViewModel model = new AdminViewModel();
            var user = UserManager.FindById(User.Identity.GetUserId());
            model.SignedInUser = user;
            var ratings = RatingServices.Instance.GetRatingsByDate(DateTime.Now);
            // Group ratings by employee

            List<ratingextra> stats = new List<ratingextra>();
            foreach (var item in ratings)
            {
                if (stats != null && stats.Count > 0)
                {
                    if (stats.Where(x => x.empName == item.Employee).Count() > 0)
                    {

                    }
                    else
                    {

                        string name = item.Employee;
                        var ratingsOfEmp = ratings.Where(x => x.Employee == name).ToList();
                        float avg = 0; int counter = 15;
                        foreach (var rate in ratingsOfEmp)
                        {
                            //counter++;
                            avg += float.Parse(rate.CustomerService) + float.Parse(rate.Expertise) + float.Parse(rate.Professionalism);
                        }
                        float finalavg = avg / counter;
                        stats.Add(new ratingextra { empName = name, ratingAVG = finalavg });
                    }
                }
                else
                {
                    string name = item.Employee;
                    var ratingsOfEmp = ratings.Where(x => x.Employee == name).ToList();
                    float avg = 0; int counter = 15;
                    foreach (var rate in ratingsOfEmp)
                    {
                        //counter++;
                        avg += float.Parse(rate.CustomerService) + float.Parse(rate.Expertise) + float.Parse(rate.Professionalism);
                    }
                    float finalavg = avg / counter;
                    var empfull = EmployeeServices.Instance.GetEmployeeByName(name);
                    stats.Add(new ratingextra {Employee=empfull ,empName = name, ratingAVG = finalavg });

                }
                //var ratingsByEmployee = ratings.GroupBy(r => r.Employee);

                //// Create a list to store the ratings for each employee
                //List<RatingFull> RatingFullList = new List<RatingFull>();

                //foreach (var group in ratingsByEmployee)
                //{

                //    // Calculate the average rating for each employee
                //    int professionalism = group.Sum(r => Convert.ToInt32(r.Professionalism));
                //    int expertise = group.Sum(r => Convert.ToInt32(r.Expertise));
                //    int customerservice = group.Sum(r => Convert.ToInt32(r.CustomerService));

                //    // Create an RatingFull object to store the employee's ratings and average
                //    RatingFull RatingFull = new RatingFull
                //    {

                //        Employee = EmployeeServices.Instance.GetEmployeeByName(group.Key),
                //        Ratings = group.ToList(),
                //        Expertise = expertise,
                //        CustomerService=customerservice,
                //        Professionalism=professionalism
                //    };

                // Add the employee's ratings to the list


               model.Statistics = stats;
            }
            return View(model);
        }



        [HttpPost]
        public ActionResult Dashboard(string SearchTerm)
        {
            AdminViewModel model = new AdminViewModel();
            var user = UserManager.FindById(User.Identity.GetUserId());
            model.SignedInUser = user;
            return View(model);
        }


    }
}