using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatingSystem.Entities
{
    public class Employee:BaseEntity
    {
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Image { get; set; }
        public string TeamName { get; set; }

    }
}
