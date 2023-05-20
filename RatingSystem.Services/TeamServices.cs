using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RatingSystem.Database;
using RatingSystem.Entities;
namespace RatingSystem.Services
{
    public class TeamServices
    {
        #region Singleton
        public static TeamServices Instance
        {
            get
            {
                if (instance == null) instance = new TeamServices();
                return instance;
            }
        }
        private static TeamServices instance { get; set; }
        private TeamServices()
        {
        }
        #endregion

        #region CRUD
        public List<Team> GetTeam(string SearchTerm = "")
        {
            using (var context = new DSContext())
            {
                if (SearchTerm != "")
                {
                    return context.Teams.Where(p => p != null && p.TeamName.ToLower()
                                            .Contains(SearchTerm.ToLower()))
                                            .OrderBy(x => x.ID)
                                            .ToList();
                }
                else
                {
                    return context.Teams.OrderBy(x => x.ID).ToList();
                }
            }
        }







        public Team GetTeam(int ID)
        {
            using (var context = new DSContext())
            {

                return context.Teams.Find(ID);

            }
        }

        public void SaveTeam(Team Team)
        {
            using (var context = new DSContext())
            {
                context.Teams.Add(Team);
                context.SaveChanges();
            }
        }

        public void UpdateTeam(Team Team)
        {
            using (var context = new DSContext())
            {
                context.Entry(Team).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteTeam(int ID)
        {
            using (var context = new DSContext())
            {

                var Team = context.Teams.Find(ID);
                context.Teams.Remove(Team);
                context.SaveChanges();
            }
        }

        #endregion
    }
}
