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
    public class EmployeeServices
    {
        #region Singleton
        public static EmployeeServices Instance
        {
            get
            {
                if (instance == null) instance = new EmployeeServices();
                return instance;
            }
        }
        private static EmployeeServices instance { get; set; }
        private EmployeeServices()
        {
        }
        #endregion

        #region CRUD
        public List<Employee> GetEmployee(string SearchTerm = "")
        {
            using (var context = new DSContext())
            {
                if (SearchTerm != "")
                {
                    return context.Employees.Where(p => p != null && p.Name.ToLower()
                                            .Contains(SearchTerm.ToLower()))
                                            .OrderBy(x => x.ID)
                                            .ToList();
                }
                else
                {
                    return context.Employees.OrderBy(x => x.ID).ToList();
                }
            }
        }

        public List<Employee> GetEmployeeByTeam(string Team,string SearchTerm = "")
        {
            using (var context = new DSContext())
            {
                if (SearchTerm != "")
                {
                    return context.Employees.Where(p => p != null && p.Name.ToLower()
                                            .Contains(SearchTerm.ToLower())
                                            && p.TeamName == Team)
                                            .OrderBy(x => x.ID)
                                            .ToList();
                }
                else
                {
                    return context.Employees.Where(x=>x.TeamName == Team).ToList();
                }
            }
        }





        public Employee GetEmployeeByName(string Name)
        {
            using (var context = new DSContext())
            {

                return context.Employees.Where(x => x.Name == Name).FirstOrDefault();

            }
        }

        public Employee GetEmployee(int ID)
        {
            using (var context = new DSContext())
            {

                return context.Employees.Find(ID);

            }
        }

        public void SaveEmployee(Employee Employee)
        {
            using (var context = new DSContext())
            {
                context.Employees.Add(Employee);
                context.SaveChanges();
            }
        }

        public void UpdateEmployee(Employee Employee)
        {
            using (var context = new DSContext())
            {
                context.Entry(Employee).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteEmployee(int ID)
        {
            using (var context = new DSContext())
            {

                var Employee = context.Employees.Find(ID);
                context.Employees.Remove(Employee);
                context.SaveChanges();
            }
        }

        #endregion
    }
}
