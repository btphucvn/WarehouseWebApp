using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using EcommerceWebsite.Helpper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NToastNotify;
using WarehouseWebApp.Models;

namespace WarehouseWebApp.Controllers
{
    public class UnitcountsController : Controller
    {
        private readonly dbwarehouseContext _context;
        private readonly IToastNotification _toastNotification;


        public UnitcountsController(dbwarehouseContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }

        // GET: Unitcounts
        public async Task<IActionResult> Index()
        {
            List<Unitcount> ls = new List<Unitcount>();
            dynamic model = new System.Dynamic.ExpandoObject();

            string url = "https://" + Request.Host.Value + "/api/apiunitcounts";

            string strJsonData = await RestAPI.GetJSON(url);
            dynamic data = JObject.Parse(strJsonData);
            string error = data["error"].ToString();

            if (error == "")
            {
                ls = JsonConvert.DeserializeObject<List<Unitcount>>(data["result"]["contents"].ToString());
                model.NumberOfPage = int.Parse(data["result"]["numberOfPage"].ToString());
                model.CurrentPage = int.Parse(data["result"]["currentPage"].ToString());
                model.UnitCounts = ls;
                model.Search = "";
            }
            return View(model);

        }
        [HttpGet]
        public async Task<IActionResult> FindUnitCount(int? page, string? search)
        {
            List<Unitcount> ls = new List<Unitcount>();
            dynamic model = new System.Dynamic.ExpandoObject();

            string url = "https://" + Request.Host.Value + "/api/apiunitcounts";
            if(page.HasValue)
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

            if(error=="")
            {
                ls = JsonConvert.DeserializeObject<List<Unitcount>>(data["result"]["contents"].ToString());
                model.NumberOfPage = int.Parse(data["result"]["numberOfPage"].ToString());
                model.CurrentPage = int.Parse(data["result"]["currentPage"].ToString());
                if(!string.IsNullOrEmpty(search))
                {
                    model.Search = search;
                }
                else
                {
                    model.Search = "";
                }
                model.UnitCounts = ls;

            }
            //List<Unitcount> categories = JsonConvert.DeserializeObject<List<Unitcount>>(strJsonData);

            if (ls != null)
            {
                return PartialView("ListUnitCountsPartial", model);
            }
            else
            {
                return PartialView("ListUnitCountsPartial", null);
            }
        }
        // GET: Unitcounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Unitcounts == null)
            {
                return NotFound();
            }

            var unitcount = await _context.Unitcounts
                .FirstOrDefaultAsync(m => m.UnitCountId == id);
            if (unitcount == null)
            {
                return NotFound();
            }

            return View(unitcount);
        }

        // GET: Unitcounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Unitcounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UnitCountId,Name")] Unitcount unitcount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(unitcount);
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Tạo thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(unitcount);
        }

        // GET: Unitcounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Unitcounts == null)
            {
                return NotFound();
            }

            var unitcount = await _context.Unitcounts.FindAsync(id);
            if (unitcount == null)
            {
                return NotFound();
            }
            return View(unitcount);
        }

        // POST: Unitcounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UnitCountId,Name")] Unitcount unitcount)
        {
            if (id != unitcount.UnitCountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(unitcount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UnitcountExists(unitcount.UnitCountId))
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
            return View(unitcount);
        }

        // GET: Unitcounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Unitcounts == null)
            {
                return NotFound();
            }

            var unitcount = await _context.Unitcounts
                .FirstOrDefaultAsync(m => m.UnitCountId == id);
            if (unitcount == null)
            {
                return NotFound();
            }

            return View(unitcount);
        }

        // POST: Unitcounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Unitcounts == null)
            {
                return Problem("Entity set 'dbwarehouseContext.Unitcounts'  is null.");
            }
            var unitcount = await _context.Unitcounts.FindAsync(id);
            if (unitcount != null)
            {
                _context.Unitcounts.Remove(unitcount);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UnitcountExists(int id)
        {
          return _context.Unitcounts.Any(e => e.UnitCountId == id);
        }
    }
}
