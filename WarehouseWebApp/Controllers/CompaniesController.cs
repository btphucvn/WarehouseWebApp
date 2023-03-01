using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceWebsite.Helpper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NToastNotify;
using WarehouseWebApp.Models;

namespace WarehouseWebApp.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly dbwarehouseContext _context;

        private readonly IToastNotification _toastNotification;


        public CompaniesController(dbwarehouseContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }

        // GET: Companies
        public async Task<IActionResult> Index()
        {
            List<Company> ls = new List<Company>();
            dynamic model = new System.Dynamic.ExpandoObject();

            string url = "https://" + Request.Host.Value + "/api/apicompanies";

            string strJsonData = await RestAPI.GetJSON(url);
            dynamic data = JObject.Parse(strJsonData);
            string error = data["error"].ToString();

            if (error == "")
            {
                ls = JsonConvert.DeserializeObject<List<Company>>(data["result"]["contents"].ToString());
                model.NumberOfPage = int.Parse(data["result"]["numberOfPage"].ToString());
                model.CurrentPage = int.Parse(data["result"]["currentPage"].ToString());
                model.Companies = ls;
                model.Search = "";
            }
            return View(model);
        }
       
        [HttpGet]
        public async Task<IActionResult> FindCompanies(int? page, string? search)
        {
            List<Company> ls = new List<Company>();
            dynamic model = new System.Dynamic.ExpandoObject();

            string url = "https://" + Request.Host.Value + "/api/apicompanies";
            if (page.HasValue)
            {
                url = url + "?page=" + page;
            }
            else
            {
                url = url + "?page=1";
            }
            if (!string.IsNullOrEmpty(search))
            {
                url = url + "&search=" + search;
            }
            string strJsonData = await RestAPI.GetJSON(url);
            dynamic data = JObject.Parse(strJsonData);
            string error = data["error"].ToString();

            if (error == "")
            {
                ls = JsonConvert.DeserializeObject<List<Company>>(data["result"]["contents"].ToString());
                model.NumberOfPage = int.Parse(data["result"]["numberOfPage"].ToString());
                model.CurrentPage = int.Parse(data["result"]["currentPage"].ToString());
                if (!string.IsNullOrEmpty(search))
                {
                    model.Search = search;
                }
                else
                {
                    model.Search = "";
                }
                model.Companies = ls;

            }
            //List<Company> categories = JsonConvert.DeserializeObject<List<Company>>(strJsonData);

            if (ls != null)
            {
                return PartialView("ListCompaniesPartial", model);
            }
            else
            {
                return PartialView("ListCompaniesPartial", null);
            }
        }


        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,CompanyCode,CompanyName,Phone,Email,Fax,Address")] Company company)
        {
            if (ModelState.IsValid)
            {
                company.Disabled = false;
                _context.Add(company);
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Tạo thành công");

                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Companies == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyId,CompanyCode,CompanyName,Phone,Email,Fax,Address,Disabled")] Company company)
        {

            if (id != company.CompanyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Sửa thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.CompanyId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            _toastNotification.AddErrorToastMessage("Sửa thất bại");

            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Companies == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Companies == null)
            {
                return Problem("Entity set 'dbwarehouseContext.Companies'  is null.");
            }
            var company = await _context.Companies.FindAsync(id);
            if (company != null)
            {
                _context.Companies.Remove(company);
            }
            
            await _context.SaveChangesAsync();
            _toastNotification.AddSuccessToastMessage("Xóa thành công");

            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
          return _context.Companies.Any(e => e.CompanyId == id);
        }
    }
}
