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
using WarehouseWebApp.Extension;

namespace WarehouseWebApp.Controllers
{
    public class GoodsController : Controller
    {
        private readonly dbwarehouseContext _context;
        private readonly IToastNotification _toastNotification;

        public GoodsController(dbwarehouseContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }

        // GET: Goods
        public async Task<IActionResult> Index()
        {
            List<Good> ls = new List<Good>();
            dynamic model = new System.Dynamic.ExpandoObject();

            string url = "https://" + Request.Host.Value + "/api/apigoods";

            string strJsonData = await RestAPI.GetJSON(url);
            dynamic data = JObject.Parse(strJsonData);
            string error = data["error"].ToString();

            if (error == "")
            {
                ls = JsonConvert.DeserializeObject<List<Good>>(data["result"]["contents"].ToString());
                model.NumberOfPage = int.Parse(data["result"]["numberOfPage"].ToString());
                model.CurrentPage = int.Parse(data["result"]["currentPage"].ToString());
                model.Goods = ls;
                model.Search = "";
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> FindGoods(int? page, string? search)
        {
            List<Good> ls = new List<Good>();
            dynamic model = new System.Dynamic.ExpandoObject();

            string url = "https://" + Request.Host.Value + "/api/apigoods";
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
                ls = JsonConvert.DeserializeObject<List<Good>>(data["result"]["contents"].ToString());
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
                model.Goods = ls;

            }
            //List<Good> categories = JsonConvert.DeserializeObject<List<Good>>(strJsonData);

            if (ls != null)
            {
                return PartialView("ListGoodsPartial", model);
            }
            else
            {
                return PartialView("ListGoodsPartial", null);
            }
        }


        // GET: Goods/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Goods == null)
            {
                return NotFound();
            }

            var good = await _context.Goods
                .Include(g => g.GroupGood)
                .Include(g => g.Supplier)
                .FirstOrDefaultAsync(m => m.Barcode == id);
            if (good == null)
            {
                return NotFound();
            }

            return View(good);
        }

        // GET: Goods/Create
        public IActionResult Create()
        {
            ViewData["GroupGoods"] = new SelectList(_context.Groupgoods, "GroupGoodId", "GroupName");
            ViewData["Suppliers"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName");
            ViewData["Origins"] = new SelectList(_context.Origins, "OriginId", "OriginName");
            ViewData["Units"] = new SelectList(_context.Units, "UnitId", "UnitName");
            ViewData["Goods"] = new SelectList(_context.Goods, "Barcode", "CategoryName");

            return View();
        }

        // POST: Goods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Barcode,CategoryName,CategoryShortName,UnitId,Price,SupplierId,OriginId,GroupGoodId,CreatedDate,CreatedBy,Disabled")] Good good)
        {
            var message = string.Join(" | ", ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage));
            if (ModelState.IsValid)
            {
                SequencesController sequencesController = new SequencesController();
                Sequence sequence = await sequencesController.GetSequence("HH@" + DateTime.Now.Year.ToString() + "@" + good.GroupGoodId.ToString());
                if (sequence == null)
                {
                    sequence = new Sequence();
                    sequence.SeqName = "HH@" + DateTime.Now.Year.ToString() + "@" + good.GroupGoodId.ToString();
                    sequence.SeqValue = 1;
                    await sequencesController.Create(sequence);
                }
                good.Barcode = BarcodeEAN13.BuildEan13(DateTime.Now.Year.ToString() + good.GroupGoodId.ToString() + sequence.SeqValue.Value.ToString("0000000"));
                good.CreatedDate = DateTime.Now;
                good.CreatedBy = 1;
                _context.Add(good);
                sequencesController.Edit(sequence);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["GroupGoods"] = new SelectList(_context.Groupgoods, "GroupGoodId", "GroupName", good.GroupGoodId);
            ViewData["Suppliers"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", good.SupplierId);
            ViewData["Origins"] = new SelectList(_context.Origins, "OriginId", "OriginName", good.OriginId);
            ViewData["Units"] = new SelectList(_context.Units, "UnitId", "UnitName", good.UnitId);

            return View(good);
        }

        // GET: Goods/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Goods == null)
            {
                return NotFound();
            }

            var good = await _context.Goods.FindAsync(id);
            if (good == null)
            {
                return NotFound();
            }
            ViewData["GroupGoods"] = new SelectList(_context.Groupgoods, "GroupGoodId", "GroupName", good.GroupGoodId);
            ViewData["Suppliers"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", good.SupplierId);
            ViewData["Origins"] = new SelectList(_context.Origins, "OriginId", "OriginName", good.OriginId);
            ViewData["Units"] = new SelectList(_context.Units, "UnitId", "UnitName", good.UnitId);

            return View(good);
        }

        // POST: Goods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Barcode,CategoryName,CategoryShortName,UnitId,Price,SupplierId,OriginId,GroupGoodId,CreatedDate,CreatedBy,Disabled")] Good good)
        {
            if (id != good.Barcode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(good);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoodExists(good.Barcode))
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
            ViewData["GroupGoods"] = new SelectList(_context.Groupgoods, "GroupGoodId", "GroupName", good.GroupGoodId);
            ViewData["Suppliers"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", good.SupplierId);
            ViewData["Origins"] = new SelectList(_context.Origins, "OriginId", "OriginName", good.OriginId);
            ViewData["Units"] = new SelectList(_context.Units, "UnitId", "UnitName", good.UnitId);

            return View(good);
        }

        // GET: Goods/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Goods == null)
            {
                return NotFound();
            }

            var good = await _context.Goods
                .Include(g => g.GroupGood)
                .Include(g => g.Supplier)
                .FirstOrDefaultAsync(m => m.Barcode == id);
            if (good == null)
            {
                return NotFound();
            }

            return View(good);
        }

        // POST: Goods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Goods == null)
            {
                return Problem("Entity set 'dbwarehouseContext.Goods'  is null.");
            }
            var good = await _context.Goods.FindAsync(id);
            if (good != null)
            {
                _context.Goods.Remove(good);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GoodExists(string id)
        {
            return _context.Goods.Any(e => e.Barcode == id);
        }
    }
}
