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
using System.Text;

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
                        int NoOfRatings = ratingsOfEmp.Count();
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

                        stats.Add(new ratingextra {NoOfRatings=NoOfRatings, empName = name, ratingAVG = finalavg, teamAVG = teamrating, customerTstar = customer_team_stars, professionalismTstar = professionalism_team_stars, expertiseTstar = expertise_team_stars, Employee = empfull, CustomerService_star = customerservice_star2, professionalism_star = professionalism_star2, expertise_star = expertise_star2 });
                    }
                }
                else
                {
                    string name = item.Employee;
                    var ratingsOfEmp = ratings.Where(x => x.Employee == name).ToList();
                    int NoOfRatings = ratingsOfEmp.Count();


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

                    stats.Add(new ratingextra { NoOfRatings= NoOfRatings, empName = name, ratingAVG = finalavg, teamAVG = teamrating, customerTstar = customer_team_stars, professionalismTstar = professionalism_team_stars, expertiseTstar = expertise_team_stars, Employee = empfull, CustomerService_star = customerservice_star2, professionalism_star = professionalism_star2, expertise_star = expertise_star2 });
                }
            }
            model.Statistics = stats;
            if (stats.Count != 0)
            {
                double rating = ratings.Count();
                double fivestar = 0;
                double fourstar = 0;
                double threestar = 0;
                double twostar = 0;
                double onestar = 0;
                foreach (var item in ratings)
                {
                    if (item.Professionalism == "5" || item.CustomerService == "5" || item.Expertise == "5")
                    {
                        fivestar++;
                    }
                    else if (item.Professionalism == "4" || item.CustomerService == "4" || item.Expertise == "4")
                    {
                        fourstar++;
                    }
                    else if (item.Professionalism == "3" || item.CustomerService == "3" || item.Expertise == "3")
                    {
                        threestar++;
                    }
                    else if (item.Professionalism == "2" || item.CustomerService == "2" || item.Expertise == "2")
                    {
                        twostar++;
                    }
                    else if (item.Professionalism == "1" || item.CustomerService == "1" || item.Expertise == "1")
                    {
                        onestar++;
                    }
                }

                model.Value.Add(fivestar);
                model.Value.Add(fourstar);
                model.Value.Add(threestar);
                model.Value.Add(twostar);
                model.Value.Add(onestar);

                model.five_star = (fivestar / ratings.Count()) * 100;
                model.four_star = (fourstar / ratings.Count()) * 100;
                model.three_star = (threestar / ratings.Count()) * 100;
                model.two_star = (twostar / ratings.Count()) * 100;
                model.one_star = (onestar / ratings.Count()) * 100;
            }
            return View(model);
        }


        public ActionResult Export()
        {

            var data = RatingServices.Instance.GetRating();


            StringBuilder csvContent = new StringBuilder();

            // Add table headers to CSV content
            csvContent.AppendLine("Employee,Team Name,Customer Service, Professionalism, Expertise,Date");

            // Add table rows from the view to CSV content
            foreach (var item in data)
            {

                csvContent.AppendLine($"{item.Employee},{item.TeamName},{item.CustomerService},{item.Professionalism},{item.Expertise},{item.Date}");
            }

            // Set response headers
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment; filename=ratingsdata.csv");

            // Write CSV content to the response
            Response.Write(csvContent.ToString());
            Response.End();

            return null;
        }
        [HttpPost]
        public ActionResult Dashboard(DateTime startDate, DateTime enddate, string type, string search_name = "")
        {

            AdminViewModel model = new AdminViewModel();
            var user = UserManager.FindById(User.Identity.GetUserId());
            model.SignedInUser = user;
            var ratings = RatingServices.Instance.GetFilteredRatings(startDate, enddate, type, search_name);

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
                        int NoOfRatings = ratingsOfEmp.Count(); //Get Me bhi check krlo ye dala hai
                        
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

                        stats.Add(new ratingextra {NoOfRatings = NoOfRatings, empName = name, ratingAVG = finalavg, teamAVG = teamrating, customerTstar = customer_team_stars, professionalismTstar = professionalism_team_stars, expertiseTstar = expertise_team_stars, Employee = empfull, CustomerService_star = customerservice_star2, professionalism_star = professionalism_star2, expertise_star = expertise_star2 });
                    }
                }
                else
                {
                    string name = item.Employee;
                    var ratingsOfEmp = ratings.Where(x => x.Employee == name).ToList();
                    int NoOfRatings = ratingsOfEmp.Count(); //Get Me bhi check krlo yeghi dala hai

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

                    stats.Add(new ratingextra {NoOfRatings = NoOfRatings, empName = name, ratingAVG = finalavg, teamAVG = teamrating, customerTstar = customer_team_stars, professionalismTstar = professionalism_team_stars, expertiseTstar = expertise_team_stars, Employee = empfull, CustomerService_star = customerservice_star2, professionalism_star = professionalism_star2, expertise_star = expertise_star2 });
                }
            }
            model.Statistics = stats;
            if (stats.Count != 0)
            {
                double rating = ratings.Count();
                double fivestar = 0;
                double fourstar = 0;
                double threestar = 0;
                double twostar = 0;
                double onestar = 0;
                foreach (var item in ratings)
                {
                    if (item.Professionalism == "5" || item.CustomerService == "5" || item.Expertise == "5")
                    {
                        fivestar++;
                    }
                    else if (item.Professionalism == "4" || item.CustomerService == "4" || item.Expertise == "4")
                    {
                        fourstar++;
                    }
                    else if (item.Professionalism == "3" || item.CustomerService == "3" || item.Expertise == "3")
                    {
                        threestar++;
                    }
                    else if (item.Professionalism == "2" || item.CustomerService == "2" || item.Expertise == "2")
                    {
                        twostar++;
                    }
                    else if (item.Professionalism == "1" || item.CustomerService == "1" || item.Expertise == "1")
                    {
                        onestar++;
                    }
                }

                model.Value.Add(fivestar);
                model.Value.Add(fourstar);
                model.Value.Add(threestar);
                model.Value.Add(twostar);
                model.Value.Add(onestar);
                model.five_star = (fivestar / ratings.Count()) * 100;
                model.four_star = (fourstar / ratings.Count()) * 100;
                model.three_star = (threestar / ratings.Count()) * 100;
                model.two_star = (twostar / ratings.Count()) * 100;
                model.one_star = (onestar / ratings.Count()) * 100;
            }

            return View(model);
        }
    }


}
