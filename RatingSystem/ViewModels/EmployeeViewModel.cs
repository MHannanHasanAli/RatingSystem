using RatingSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatingSystem.ViewModels
{
    public class EmployeeListingViewModel
    {
        public List<Employee> Employees { get; set; }
        public string SearchTerm { get; set; }
    }

    public class EmployeeActionViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Image { get; set; }
        public string TeamName { get; set; }
    }
}