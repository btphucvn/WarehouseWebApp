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
using Microsoft.CodeAnalysis;

namespace WarehouseWebApp.Controllers
{
    public class DocumentdetailsController : Controller
    {
        private readonly dbwarehouseContext _context;

        public DocumentdetailsController(dbwarehouseContext context)
        {
            _context = context;
        }

        // GET: Documentdetails
        public async Task<IActionResult> Index()
        {
            var dbwarehouseContext = _context.Documentdetails.Include(d => d.Document);
            return View(await dbwarehouseContext.ToListAsync());
        }


        // GET: Documentdetails/Create
        public IActionResult Create(int id)
        {
            Documentdetail documentdetail = new Documentdetail();
            documentdetail.DocumentId = id;

            ViewData["Goods"] = new SelectList(_context.Goods, "Barcode", "CategoryName");

            return View(documentdetail);
        }
        [HttpGet]
        public async Task<IActionResult> FindDocumentdetails(int? page, int documentID)
        {
            List<Documentdetail> ls = new List<Documentdetail>();
            dynamic model = new System.Dynamic.ExpandoObject();

            string url = "https://" + Request.Host.Value + "/api/apiDocumentdetails";
            if (page.HasValue)
            {
                url = url + "?page=" + page;
            }
            else
            {
                url = url + "?page=1";
            }
            url = url + "&documentid=" + documentID;

            string strJsonData = await RestAPI.GetJSON(url);
            dynamic data = JObject.Parse(strJsonData);
            string error = data["error"].ToString();

            if (error == "")
            {
                ls = JsonConvert.DeserializeObject<List<Documentdetail>>(data["result"]["contents"].ToString());
                model.NumberOfPage = int.Parse(data["result"]["numberOfPage"].ToString());
                model.CurrentPage = int.Parse(data["result"]["currentPage"].ToString());
                model.Documentdetails = ls;
                model.Search = "";

            }
            //List<Documentdetail> categories = JsonConvert.DeserializeObject<List<Documentdetail>>(strJsonData);

            if (ls != null)
            {
                return PartialView("ListDocumentdetailsPartial", model);
            }
            else
            {
                return PartialView("ListDocumentdetailsPartial", null);
            }
        }

        // POST: Documentdetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DocumentDetailId,DocumentId,Barcode,Quantity,Price,TotalAmount,CreatedDate")] Documentdetail documentdetail)
        {
            if (ModelState.IsValid)
            {
                documentdetail.TotalAmount = documentdetail.Price * documentdetail.Quantity;
                _context.Add(documentdetail);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", new { id = documentdetail.DocumentId });
            }
            ViewData["Goods"] = new SelectList(_context.Goods, "Barcode", "CategoryName");

            return View(documentdetail);
        }

        // GET: Documentdetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Documentdetails == null)
            {
                return NotFound();
            }

            var documentdetail = await _context.Documentdetails.FindAsync(id);
            if (documentdetail == null)
            {
                return NotFound();
            }
            ViewData["Goods"] = new SelectList(_context.Goods, "Barcode", "CategoryName",documentdetail.Barcode);

            return View(documentdetail);
        }

        // POST: Documentdetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DocumentDetailId,DocumentId,Barcode,Quantity,Price,TotalAmount,CreatedDate")] Documentdetail documentdetail)
        {
            if (id != documentdetail.DocumentDetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(documentdetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentdetailExists(documentdetail.DocumentDetailId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Create", new { id = documentdetail.DocumentId });
            }
            ViewData["Goods"] = new SelectList(_context.Goods, "Barcode", "CategoryName", documentdetail.Barcode);
            return View(documentdetail);
        }

        // GET: Documentdetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Documentdetails == null)
            {
                return NotFound();
            }

            var documentdetail = await _context.Documentdetails
                .Include(d => d.Document)
                .FirstOrDefaultAsync(m => m.DocumentDetailId == id);
            if (documentdetail == null)
            {
                return NotFound();
            }

            return View(documentdetail);
        }

        // POST: Documentdetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Documentdetails == null)
            {
                return Problem("Entity set 'dbwarehouseContext.Documentdetails'  is null.");
            }
            var documentdetail = await _context.Documentdetails.FindAsync(id);
            if (documentdetail != null)
            {
                _context.Documentdetails.Remove(documentdetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Create", new { id = documentdetail.DocumentId });

        }

        private bool DocumentdetailExists(int id)
        {
          return _context.Documentdetails.Any(e => e.DocumentDetailId == id);
        }
    }
}
