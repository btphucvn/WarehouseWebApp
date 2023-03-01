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
    public class OriginsController : Controller
    {
        private readonly dbwarehouseContext _context;
        private readonly IToastNotification _toastNotification;

        public OriginsController(dbwarehouseContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }

        // GET: Origins
        public async Task<IActionResult> Index()
        {
            List<Origin> ls = new List<Origin>();
            dynamic model = new System.Dynamic.ExpandoObject();

            string url = "https://" + Request.Host.Value + "/api/apiOrigins";

            string strJsonData = await RestAPI.GetJSON(url);
            dynamic data = JObject.Parse(strJsonData);
            string error = data["error"].ToString();

            if (error == "")
            {
                ls = JsonConvert.DeserializeObject<List<Origin>>(data["result"]["contents"].ToString());
                model.NumberOfPage = int.Parse(data["result"]["numberOfPage"].ToString());
                model.CurrentPage = int.Parse(data["result"]["currentPage"].ToString());
                model.Origins = ls;
                model.Search = "";
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> FindOrigins(int? page, string? search)
        {
            List<Origin> ls = new List<Origin>();
            dynamic model = new System.Dynamic.ExpandoObject();

            string url = "https://" + Request.Host.Value + "/api/apiOrigins";
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
                ls = JsonConvert.DeserializeObject<List<Origin>>(data["result"]["contents"].ToString());
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
                model.Origins = ls;

            }
            //List<Origin> categories = JsonConvert.DeserializeObject<List<Origin>>(strJsonData);

            if (ls != null)
            {
                return PartialView("ListOriginsPartial", model);
            }
            else
            {
                return PartialView("ListOriginsPartial", null);
            }
        }


        // GET: Origins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Origins == null)
            {
                return NotFound();
            }

            var origin = await _context.Origins
                .FirstOrDefaultAsync(m => m.OriginId == id);
            if (origin == null)
            {
                return NotFound();
            }

            return View(origin);
        }

        // GET: Origins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Origins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OriginId,OriginName")] Origin origin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(origin);
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Tạo thành công");

                return RedirectToAction(nameof(Index));
            }
            return View(origin);
        }

        // GET: Origins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Origins == null)
            {
                return NotFound();
            }

            var origin = await _context.Origins.FindAsync(id);
            if (origin == null)
            {
                return NotFound();
            }
            return View(origin);
        }

        // POST: Origins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OriginId,OriginName")] Origin origin)
        {
            if (id != origin.OriginId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(origin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OriginExists(origin.OriginId))
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
            return View(origin);
        }

        // GET: Origins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Origins == null)
            {
                return NotFound();
            }

            var origin = await _context.Origins
                .FirstOrDefaultAsync(m => m.OriginId == id);
            if (origin == null)
            {
                return NotFound();
            }

            return View(origin);
        }

        // POST: Origins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Origins == null)
            {
                return Problem("Entity set 'dbwarehouseContext.Origins'  is null.");
            }
            var origin = await _context.Origins.FindAsync(id);
            if (origin != null)
            {
                _context.Origins.Remove(origin);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OriginExists(int id)
        {
          return _context.Origins.Any(e => e.OriginId == id);
        }
    }
}
