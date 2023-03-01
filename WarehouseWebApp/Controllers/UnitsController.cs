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
using WarehouseWebApp.Models;
using NToastNotify;

namespace WarehouseWebApp.Controllers
{
    public class UnitsController : Controller
    {
        private readonly dbwarehouseContext _context;
        private readonly IToastNotification _toastNotification;

        public UnitsController(dbwarehouseContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        // GET: Units
        public async Task<IActionResult> Index()
        {
            List<Unit> ls = new List<Unit>();
            dynamic model = new System.Dynamic.ExpandoObject();

            string url = "https://" + Request.Host.Value + "/api/apiunits";

            string strJsonData = await RestAPI.GetJSON(url);
            dynamic data = JObject.Parse(strJsonData);
            string error = data["error"].ToString();

            if (error == "")
            {
                ls = JsonConvert.DeserializeObject<List<Unit>>(data["result"]["contents"].ToString());
                model.NumberOfPage = int.Parse(data["result"]["numberOfPage"].ToString());
                model.CurrentPage = int.Parse(data["result"]["currentPage"].ToString());
                model.Units = ls;
                model.Search = "";
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> FindUnits(int? page, string? search)
        {
            List<Unit> ls = new List<Unit>();
            dynamic model = new System.Dynamic.ExpandoObject();

            string url = "https://" + Request.Host.Value + "/api/apiunits";
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
                ls = JsonConvert.DeserializeObject<List<Unit>>(data["result"]["contents"].ToString());
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
                model.Units = ls;

            }
            //List<Unit> categories = JsonConvert.DeserializeObject<List<Unit>>(strJsonData);

            if (ls != null)
            {
                return PartialView("ListUnitsPartial", model);
            }
            else
            {
                return PartialView("ListUnitsPartial", null);
            }
        }
        // GET: Units/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Units == null)
            {
                return NotFound();
            }

            var unit = await _context.Units
                .Include(u => u.Company)
                .FirstOrDefaultAsync(m => m.UnitId == id);
            if (unit == null)
            {
                return NotFound();
            }

            return View(unit);
        }

        // GET: Units/Create
        public IActionResult Create()
        {
            ViewData["Companies"] = new SelectList(_context.Companies, "CompanyId", "CompanyName");
            return View();
        }

        // POST: Units/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UnitId,CompanyId,UnitCode,UnitName,Phone,Email,Fax,Address,Disabled,Inventory")] Unit unit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(unit);
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Thêm thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["Companies"] = new SelectList(_context.Companies, "CompanyId", "CompanyName");
            return View(unit);
        }

        // GET: Units/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Units == null)
            {
                return NotFound();
            }

            var unit = await _context.Units.FindAsync(id);
            if (unit == null)
            {
                return NotFound();
            }
            ViewData["Companies"] = new SelectList(_context.Companies, "CompanyId", "CompanyName");
            return View(unit);
        }

        // POST: Units/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UnitId,CompanyId,UnitCode,UnitName,Phone,Email,Fax,Address,Disabled,Inventory")] Unit unit)
        {
            if (id != unit.UnitId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(unit);
                    _toastNotification.AddSuccessToastMessage("Sửa thành công");

                    await _context.SaveChangesAsync();
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UnitExists(unit.UnitId))
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
            ViewData["Companies"] = new SelectList(_context.Companies, "CompanyId", "CompanyName");
            return View(unit);
        }

        // GET: Units/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Units == null)
            {
                return NotFound();
            }

            var unit = await _context.Units
                .Include(u => u.Company)
                .FirstOrDefaultAsync(m => m.UnitId == id);
            if (unit == null)
            {
                return NotFound();
            }

            return View(unit);
        }

        // POST: Units/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Units == null)
            {
                _toastNotification.AddSuccessToastMessage("Xóa thất bại");
                return Problem("Entity set 'dbwarehouseContext.Units'  is null.");
            }
            var unit = await _context.Units.FindAsync(id);
            if (unit != null)
            {
                _context.Units.Remove(unit);
            }
            
            await _context.SaveChangesAsync();
            _toastNotification.AddSuccessToastMessage("Xóa thành công");

            return RedirectToAction(nameof(Index));
        }

        private bool UnitExists(int id)
        {
          return _context.Units.Any(e => e.UnitId == id);
        }
    }
}
