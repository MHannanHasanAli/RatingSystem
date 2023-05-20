using RatingSystem.Entities;
using RatingSystem.Services;
using RatingSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RatingSystem.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index(string SearchTerm ="")
        {
            EmployeeListingViewModel model = new EmployeeListingViewModel();
            model.Employees = EmployeeServices.Instance.GetEmployee(SearchTerm);
            return View(model);
        }


        [HttpGet]
        public ActionResult Action(int ID = 0)
        {
            EmployeeActionViewModel model = new EmployeeActionViewModel();  
            if(ID != 0)
            {
                var employee = EmployeeServices.Instance.GetEmployee(ID);
                model.ID = employee.ID;
                model.Name = employee.Name;
                model.TeamName = employee.TeamName;
                model.Image = employee.Image;
                model.Designation = employee.Designation;
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult Action(EmployeeActionViewModel model)
        {
            if(model.ID != 0) 
            {
                var employee = EmployeeServices.Instance.GetEmployee(model.ID);
                employee.ID = model.ID;
                employee.Name = model.Name;
                employee.Designation = model.Designation;
                employee.TeamName = model.TeamName;
                employee.Image = model.Image;
                EmployeeServices.Instance.UpdateEmployee(employee);
            }
            else
            {
                var employee = new Employee();
                employee.Name = model.Name;
                employee.Designation = model.Designation;
                employee.TeamName = model.TeamName;
                employee.Image = model.Image;
                EmployeeServices.Instance.SaveEmployee(employee);
            }
            return Json(new {success=true},JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Delete(int ID)
        {
            EmployeeActionViewModel model = new EmployeeActionViewModel();
            var employee = EmployeeServices.Instance.GetEmployee(ID);
            model.ID = employee.ID;
            model.Name = employee.Name;
            return PartialView("_Delete", model);
        }


        [HttpPost]
        public ActionResult Delete(EmployeeActionViewModel model)
        {
            var employee = EmployeeServices.Instance.GetEmployee(model.ID);
            EmployeeServices.Instance.DeleteEmployee(employee.ID);
            return Json(new {success=true},JsonRequestBehavior.AllowGet);
        }
    }
}