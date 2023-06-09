using RatingSystem.Database;
using RatingSystem.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatingSystem.Services
{
    public class RatingServices
    {
        #region Singleton
        public static RatingServices Instance
        {
            get
            {
                if (instance == null) instance = new RatingServices();
                return instance;
            }
        }
        private static RatingServices instance { get; set; }
        private RatingServices()
        {
        }
        #endregion

        #region CRUD
        public List<Rating> GetRating(string SearchTerm = "")
        {
            using (var context = new DSContext())
            {
                if (SearchTerm != "")
                {
                    return context.Ratings.Where(p => p != null && p.Employee.ToLower()
                                            .Contains(SearchTerm.ToLower()))
                                            .OrderBy(x => x.ID)
                                            .ToList();
                }
                else
                {
                    return context.Ratings.OrderBy(x => x.ID).ToList();
                }
            }
        }
        
        //Correct This
        public List<Rating> GetRatingsByDate(DateTime startDate)
        {

            DateTime extractedDate = startDate.Date;

            using (var context = new DSContext())
            {
                return context.Ratings
                    .Where(x => x.Date >= extractedDate)
                    .OrderBy(x => x.ID)
                    .ToList();
            }
        }

        public List<Rating> GetFilteredRatings(DateTime startDate, DateTime enddate, string type,string name="")
        {

            DateTime extractedDate = startDate.Date;

            using (var context = new DSContext())
            {

                if (type == "Employee")
                {
                    if (name != "")
                    {
                        return context.Ratings
                        .Where(x => x != null && name.ToLower().Contains(x.Employee.ToLower()) && x.Date >= startDate && x.Date <= enddate)
                        .OrderBy(x => x.ID)
                        .ToList();
                    }
                    else
                    {
                        return context.Ratings
                       .Where(x => x.Date >= startDate && x.Date <= enddate)
                       .OrderBy(x => x.ID)
                       .ToList();
                    }
                    
                }
                else
                {
                    if (name != "")
                    {
                        return context.Ratings
                        .Where(x => x != null && name.ToLower().Contains(x.TeamName.ToLower()) && x.Date >= startDate && x.Date <= enddate)
                        .OrderBy(x => x.ID)
                        .ToList();
                    }
                    else
                    {
                        return context.Ratings
                       .Where(x => x.Date >= startDate && x.Date <= enddate)
                       .OrderBy(x => x.ID)
                       .ToList();
                    }
                }
            }
        }

        public List<Rating> GetRatingByEmployee(string Employee)
        {
            using (var context = new DSContext())
            {
               
                    return context.Ratings.Where(x=>x.Employee == Employee).ToList();
                
            }
        }

        public List<Rating> GetRatingByTeam(string Team)
        {
            using (var context = new DSContext())
            {
                return context.Ratings.Where(x => x.TeamName == Team).ToList();

            }
        }







        public Rating GetRating(int ID)
        {
            using (var context = new DSContext())
            {

                return context.Ratings.Find(ID);

            }
        }

        public void SaveRating(Rating Rating)
        {
            using (var context = new DSContext())
            {
                context.Ratings.Add(Rating);
                context.SaveChanges();
            }
        }

        public void UpdateRating(Rating Rating)
        {
            using (var context = new DSContext())
            {
                context.Entry(Rating).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteRating(int ID)
        {
            using (var context = new DSContext())
            {

                var Rating = context.Ratings.Find(ID);
                context.Ratings.Remove(Rating);
                context.SaveChanges();
            }
        }

        public Rating Getip(string ip)
        {
            using(var context = new DSContext())
            {

                return context.Ratings.Find(ip);


            }
        }
        #endregion
    }
}
