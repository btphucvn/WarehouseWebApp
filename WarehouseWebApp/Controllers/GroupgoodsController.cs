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
    public class GroupgoodsController : Controller
    {
        private readonly dbwarehouseContext _context;
        private readonly IToastNotification _toastNotification;

        public GroupgoodsController(dbwarehouseContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }

        // GET: Groupgoods
        public async Task<IActionResult> Index()
        {
            List<Groupgood> ls = new List<Groupgood>();
            dynamic model = new System.Dynamic.ExpandoObject();

            string url = "https://" + Request.Host.Value + "/api/apiGroupgoods";

            string strJsonData = await RestAPI.GetJSON(url);
            dynamic data = JObject.Parse(strJsonData);
            string error = data["error"].ToString();

            if (error == "")
            {
                ls = JsonConvert.DeserializeObject<List<Groupgood>>(data["result"]["contents"].ToString());
                model.NumberOfPage = int.Parse(data["result"]["numberOfPage"].ToString());
                model.CurrentPage = int.Parse(data["result"]["currentPage"].ToString());
                model.Groupgoods = ls;
                model.Search = "";
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> FindGroupgoods(int? page, string? search)
        {
            List<Groupgood> ls = new List<Groupgood>();
            dynamic model = new System.Dynamic.ExpandoObject();

            string url = "https://" + Request.Host.Value + "/api/apiGroupgoods";
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
                ls = JsonConvert.DeserializeObject<List<Groupgood>>(data["result"]["contents"].ToString());
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
                model.Groupgoods = ls;

            }
            //List<Groupgood> categories = JsonConvert.DeserializeObject<List<Groupgood>>(strJsonData);

            if (ls != null)
            {
                return PartialView("ListGroupgoodsPartial", model);
            }
            else
            {
                return PartialView("ListGroupgoodsPartial", null);
            }
        }

        // GET: Groupgoods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Groupgoods == null)
            {
                return NotFound();
            }

            var groupgood = await _context.Groupgoods
                .FirstOrDefaultAsync(m => m.GroupGoodId == id);
            if (groupgood == null)
            {
                return NotFound();
            }

            return View(groupgood);
        }

        // GET: Groupgoods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Groupgoods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupGoodId,GroupName")] Groupgood groupgood)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupgood);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(groupgood);
        }

        // GET: Groupgoods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Groupgoods == null)
            {
                return NotFound();
            }

            var groupgood = await _context.Groupgoods.FindAsync(id);
            if (groupgood == null)
            {
                return NotFound();
            }
            return View(groupgood);
        }

        // POST: Groupgoods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GroupGoodId,GroupName")] Groupgood groupgood)
        {
            if (id != groupgood.GroupGoodId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupgood);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupgoodExists(groupgood.GroupGoodId))
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
            return View(groupgood);
        }

        // GET: Groupgoods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Groupgoods == null)
            {
                return NotFound();
            }

            var groupgood = await _context.Groupgoods
                .FirstOrDefaultAsync(m => m.GroupGoodId == id);
            if (groupgood == null)
            {
                return NotFound();
            }

            return View(groupgood);
        }

        // POST: Groupgoods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Groupgoods == null)
            {
                return Problem("Entity set 'dbwarehouseContext.Groupgoods'  is null.");
            }
            var groupgood = await _context.Groupgoods.FindAsync(id);
            if (groupgood != null)
            {
                _context.Groupgoods.Remove(groupgood);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupgoodExists(int id)
        {
          return _context.Groupgoods.Any(e => e.GroupGoodId == id);
        }
    }
}
