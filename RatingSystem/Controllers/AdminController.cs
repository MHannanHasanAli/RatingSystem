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
                        List<float> total = new List<float>();
                        int customerservice_star = 0;
                        int professionalism_star = 0;
                        int expertise_star = 0;
                        foreach (var rate in ratingsOfEmp)
                        {
                            //multiple stars added by different accounts
                            avg += float.Parse(rate.CustomerService) + float.Parse(rate.Expertise) + float.Parse(rate.Professionalism);
                            
                            customerservice_star += int.Parse(rate.CustomerService);
                            professionalism_star += int.Parse(rate.Professionalism);
                            expertise_star += int.Parse(rate.Expertise);
                        }

                        float finalavg = (avg / 3) / ratingsOfEmp.Count();
                        ////team rating
                        total.Add(finalavg);
                        float teamrating_numenator = total.Sum(x => Convert.ToInt32(x));
                        float teamrating = teamrating_numenator / total.Count();
                        ////team rating
                        int customerservice_star2 = customerservice_star / ratingsOfEmp.Count();
                        int professionalism_star2 = professionalism_star / ratingsOfEmp.Count();
                       int expertise_star2 = expertise_star / ratingsOfEmp.Count();
                        ////team rating star
                        //List<int> team_customer_star = new List<int>();
                        //List<int> team_professionalism_star = new List<int>();
                        //List<int> team_expertise_star = new List<int>();
                        ////team rating star

                        ////team rating star looped
                        //team_customer_star.Add(customerservice_star2);
                        //team_professionalism_star.Add(professionalism_star2);
                        //team_expertise_star.Add(expertise_star2);

                        ////team rating star looped

                        ////team rating star
                        //int teamrating_customer_star_numenator = team_customer_star.Sum(x => Convert.ToInt32(x));
                        //int teamrating_customer_star = teamrating_customer_star_numenator / team_customer_star.Count();
                        //int teamrating_professionalism_star_numenator = team_professionalism_star.Sum(x => Convert.ToInt32(x));
                        //int teamrating_professionalism_star = teamrating_professionalism_star_numenator / team_professionalism_star.Count();
                        //int teamrating_expertise_star_numenator = team_expertise_star.Sum(x => Convert.ToInt32(x));
                        //int teamrating_expertise_star = teamrating_expertise_star_numenator / team_expertise_star.Count();
                        ////team rating star
                        var empfull = EmployeeServices.Instance.GetEmployeeByName(name);
                        
                        stats.Add(new ratingextra { empName = name, ratingAVG = finalavg,teamAVG=teamrating,Employee =empfull, CustomerService_star = customerservice_star2, professionalism_star = professionalism_star2, expertise_star=expertise_star2 });
                    }
                }
                else
                {
                    string name = item.Employee;
                    var ratingsOfEmp = ratings.Where(x => x.Employee == name).ToList();
                    float avg = 0; int counter = 15;
                    List<float> total = new List<float>();
                    int customerservice_star = 0;
                    int professionalism_star = 0;
                    int expertise_star = 0;
                    foreach (var rate in ratingsOfEmp)
                    {
                        //multiple stars added by different accounts
                        avg += float.Parse(rate.CustomerService) + float.Parse(rate.Expertise) + float.Parse(rate.Professionalism);

                        customerservice_star += int.Parse(rate.CustomerService);
                        professionalism_star += int.Parse(rate.Professionalism);
                        expertise_star += int.Parse(rate.Expertise);
                    }
                  
                    float finalavg = avg / counter;
                    //team rating
                    total.Add(finalavg);
                    float teamrating_numenator = total.Sum(x => Convert.ToInt32(x));
                    float teamrating = teamrating_numenator / total.Count();
                    //team rating
                    int customerservice_star2 = customerservice_star / 5;
                    int professionalism_star2 = professionalism_star / 5;
                    int expertise_star2 = expertise_star / 5;
                    var empfull = EmployeeServices.Instance.GetEmployeeByName(name);

                    stats.Add(new ratingextra { empName = name, ratingAVG = finalavg, teamAVG = teamrating, Employee = empfull, CustomerService_star = customerservice_star2, professionalism_star = professionalism_star2, expertise_star = expertise_star2 });
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