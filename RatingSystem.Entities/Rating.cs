﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatingSystem.Entities
{
    public class Rating:BaseEntity
    {
        public string Employee { get; set; }
        public string TeamName { get; set; }
        public string CustomerService { get; set; }
        public string Professionalism { get; set; }
        public string Expertise { get; set; }
        public DateTime Date { get; set; }
    }
}
