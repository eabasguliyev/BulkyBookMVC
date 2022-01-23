using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }



        public IActionResult Upsert(int? id)
        {
            Company company = null;

            if (id == null || id == 0)
            {
                // Create Company
                company = new Company();
            }
            else
            {
                company = _unitOfWork.Company.GetFirstOrDefault(c => c.Id == id);
                // update product
            }

            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if(company.Id == 0)
                {
                    _unitOfWork.Company.Add(company);
                    TempData["success"] = "Company created successfully";
                }
                else
                {
                    _unitOfWork.Company.Update(company);
                    TempData["success"] = "Company updated successfully";
                }
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }

            return View(company);
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.Company.GetAll();

            return Json(new { data = companyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var company = _unitOfWork.Company.GetFirstOrDefault(c => c.Id == id);

            if (company == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Error while deleting"
                });
            }

            _unitOfWork.Company.Remove(company);
            _unitOfWork.Save();

            return Json(new
            {
                success = true,
                message = "Delete Successful"
            });
        }

        #endregion
    }
}
