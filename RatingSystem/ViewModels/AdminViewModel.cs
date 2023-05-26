using RatingSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RatingSystem.ViewModels
{
    public class AdminViewModel
    {
        public User SignedInUser { get; set; }
        public string Name { get; set; }

        public string ID { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Contact { get; set; }
        public string SearchTerm { get; set; }

        public double one_star { get; set; }
        public double two_star { get; set; }
        public double three_star { get; set; }
        public double four_star { get; set; }
        public double five_star { get; set; }

   
        public List<double> Value = new List<double>();

        public List<ratingextra> Statistics { get; set; }

    }

 

    public class RatingFull
    {
        public Employee Employee { get; set; }
        public List<Rating> Ratings { get; set; }
        public int Professionalism { get; set; }
        public int Expertise { get; set; }
        public int CustomerService { get; set; }
    }

    public class ratingextra
    {
        public Employee Employee { get; set; }
        public int NoOfRatings { get; set; }

        public string  empName { get; set; }
        public float ratingAVG { get; set; }
        public float CustomerService{ get; set; }
        public float professionalism { get; set; }
        public float expertise { get; set; }

        public int CustomerService_star { get; set; }
        public int professionalism_star { get; set; }
        public int expertise_star { get; set; }

        public int customerTstar { get; set; }
        public int professionalismTstar { get; set; }
        public int expertiseTstar { get; set; }
        public float teamAVG { get; set; }
        

    }
}

