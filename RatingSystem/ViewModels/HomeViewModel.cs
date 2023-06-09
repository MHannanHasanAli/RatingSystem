using RatingSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatingSystem.ViewModels
{
    public class HomeViewModel
    {
        public List<Employee> Employees { get; set; }
        public string BoxName { get; set; }
        public string BoxDesignation { get; set; }
    }

    public class RatingViewModel
    {
        public Employee EmployeeFull { get; set; }
        public string Employee { get; set; }
        public string TeamName { get; set; }
        public string CustomerService { get; set; }
        public string Professionalism { get; set; }
        public string Expertise { get; set; }
        public string Respect { get; set; }
        public string Explanation { get; set; }
        public string Treatment { get; set; }
        public string Overall { get; set; }
        public string Address { get; set; }
    }
}