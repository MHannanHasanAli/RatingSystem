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

            List<float> total = new List<float>();
            List<int> team_customer_star = new List<int>();
            List<int> team_professionalism_star = new List<int>();
            List<int> team_expertise_star = new List<int>();

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
                        float avg = 0;
                        int customerservice_star = 0;
                        int professionalism_star = 0;
                        int expertise_star = 0;
                        foreach (var rate in ratingsOfEmp)
                        {
                            avg += float.Parse(rate.CustomerService) + float.Parse(rate.Expertise) + float.Parse(rate.Professionalism);
                            
                            customerservice_star += int.Parse(rate.CustomerService);
                            professionalism_star += int.Parse(rate.Professionalism);
                            expertise_star += int.Parse(rate.Expertise);
                        }

                        float finalavg = (avg / 3) / ratingsOfEmp.Count();

                        total.Add(finalavg);
                        float teamrating = total.Sum(x => x) / total.Count();
                       
                        int customerservice_star2 = customerservice_star / ratingsOfEmp.Count();
                        int professionalism_star2 = professionalism_star / ratingsOfEmp.Count();
                        int expertise_star2 = expertise_star / ratingsOfEmp.Count();

                        team_customer_star.Add(customerservice_star2);
                        team_professionalism_star.Add(professionalism_star2);
                        team_expertise_star.Add(expertise_star2);

                        int customer_team_stars = team_customer_star.Sum(x=>x)/team_customer_star.Count();
                        int professionalism_team_stars = team_professionalism_star.Sum(x => x) / team_professionalism_star.Count();
                        int expertise_team_stars = team_expertise_star.Sum(x => x) / team_expertise_star.Count();

                        var empfull = EmployeeServices.Instance.GetEmployeeByName(name);
                        
                        stats.Add(new ratingextra { empName = name, ratingAVG = finalavg,teamAVG=teamrating,customerTstar=customer_team_stars,professionalismTstar=professionalism_team_stars,expertiseTstar=expertise_team_stars,Employee =empfull, CustomerService_star = customerservice_star2, professionalism_star = professionalism_star2, expertise_star=expertise_star2 });
                    }
                }
                else
                {
                    string name = item.Employee;
                    var ratingsOfEmp = ratings.Where(x => x.Employee == name).ToList();
                    float avg = 0;
                    int customerservice_star = 0;
                    int professionalism_star = 0;
                    int expertise_star = 0;
                    foreach (var rate in ratingsOfEmp)
                    {
                        avg += float.Parse(rate.CustomerService) + float.Parse(rate.Expertise) + float.Parse(rate.Professionalism);

                        customerservice_star += int.Parse(rate.CustomerService);
                        professionalism_star += int.Parse(rate.Professionalism);
                        expertise_star += int.Parse(rate.Expertise);
                    }

                    float finalavg = (avg / 3) / ratingsOfEmp.Count();

                    total.Add(finalavg);
                    float teamrating = total.Sum(x => x) / total.Count();

                    int customerservice_star2 = customerservice_star / ratingsOfEmp.Count();
                    int professionalism_star2 = professionalism_star / ratingsOfEmp.Count();
                    int expertise_star2 = expertise_star / ratingsOfEmp.Count();

                    team_customer_star.Add(customerservice_star2);
                    team_professionalism_star.Add(professionalism_star2);
                    team_expertise_star.Add(expertise_star2);

                    int customer_team_stars = team_customer_star.Sum(x => x) / team_customer_star.Count();
                    int professionalism_team_stars = team_professionalism_star.Sum(x => x) / team_professionalism_star.Count();
                    int expertise_team_stars = team_expertise_star.Sum(x => x) / team_expertise_star.Count();

                    var empfull = EmployeeServices.Instance.GetEmployeeByName(name);

                    stats.Add(new ratingextra { empName = name, ratingAVG = finalavg, teamAVG = teamrating, customerTstar = customer_team_stars, professionalismTstar = professionalism_team_stars, expertiseTstar = expertise_team_stars, Employee = empfull, CustomerService_star = customerservice_star2, professionalism_star = professionalism_star2, expertise_star = expertise_star2 });
                }
            }
            model.Statistics = stats;
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