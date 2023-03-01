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

namespace WarehouseWebApp.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly dbwarehouseContext _context;

        public DocumentsController(dbwarehouseContext context)
        {
            _context = context;
        }

        // GET: Documents
        public async Task<IActionResult> Index()
        {

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> FindDocuments(int? page, string? search, int documenttype)
        {
            List<Document> ls = new List<Document>();
            dynamic model = new System.Dynamic.ExpandoObject();

            string url = "https://" + Request.Host.Value + "/api/apiDocuments";
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
            url = url + "&documenttype=" + documenttype;

            string strJsonData = await RestAPI.GetJSON(url);
            dynamic data = JObject.Parse(strJsonData);
            string error = data["error"].ToString();
            if (error == "")
            {
                ls = JsonConvert.DeserializeObject<List<Document>>(data["result"]["contents"].ToString());
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
                model.Documents = ls;

            }
            //List<Document> categories = JsonConvert.DeserializeObject<List<Document>>(strJsonData);

            if (ls != null)
            {
                return PartialView("ListDocumentsPartial", model);
            }
            else
            {
                return PartialView("ListDocumentsPartial", null);
            }
        }



        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Documents == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.Company)
                .Include(d => d.Unit)
                .FirstOrDefaultAsync(m => m.DocumentId == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Documents/Create
        public IActionResult Create()
        {
            ViewData["Companies"] = new SelectList(_context.Companies, "CompanyId", "CompanyName");
            ViewData["Suppliers"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName");

            ViewData["Units"] = new SelectList(_context.Units, "UnitId", "UnitName");
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DocumentId,DocumentCode,DocumentNumber,DateDocument,DocumentNumber2,DateDocument2,SupplierId,Quantity,TotalAmount,Note,CompanyId,UnitId,Status,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,DeleteDate,DeletedBy")] Document document)
        {
            if (ModelState.IsValid)
            {
                SequencesController sequenceController = new SequencesController();
                Unit unit = await _context.Units.FindAsync(document.UnitId);
                Sequence sequence = await sequenceController.GetSequence("BUY@" + DateTime.Today.Year.ToString() + "@" + unit.UnitCode);
                if (sequence == null)
                {
                    sequence= new Sequence();
                    sequence.SeqName = "BUY@" + DateTime.Today.Year.ToString() + "@" + unit.UnitCode;
                    sequence.SeqValue = 1;
                    sequenceController.Create(sequence);
                }
                document.DocumentType = 1;
                document.DocumentNumber=sequence.SeqValue.Value.ToString("000000")+@"/"+DateTime.Today.Year.ToString().Substring(2,2)+@"/BUY/"+ unit.UnitCode;
                _context.Add(document);
                await _context.SaveChangesAsync();
                sequenceController.Edit(sequence);
                return RedirectToAction(nameof(Index));
            }
            ViewData["Companies"] = new SelectList(_context.Companies, "CompanyId", "CompanyName", document.CompanyId);
            ViewData["Suppliers"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", document.SupplierId);
            ViewData["Units"] = new SelectList(_context.Units, "UnitId", "UnitName", document.UnitId);
            return View(document);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Documents == null)
            {
                return NotFound();
            }

            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            ViewData["Companies"] = new SelectList(_context.Companies, "CompanyId", "CompanyName", document.CompanyId);
            ViewData["Suppliers"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", document.SupplierId);
            ViewData["Units"] = new SelectList(_context.Units, "UnitId", "UnitName", document.UnitId);
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DocumentId,DocumentCode,DocumentNumber,DateDocument,DocumentNumber2,DateDocument2,SupplierId,Quantity,TotalAmount,Note,CompanyId,UnitId,Status,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,DeleteDate,DeletedBy")] Document document)
        {
            if (id != document.DocumentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {


                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.DocumentId))
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
            ViewData["Companies"] = new SelectList(_context.Companies, "CompanyId", "CompanyName", document.CompanyId);
            ViewData["Suppliers"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", document.SupplierId);
            ViewData["Units"] = new SelectList(_context.Units, "UnitId", "UnitName", document.UnitId);
            return View(document);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Documents == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.Company)
                .Include(d => d.Unit)
                .FirstOrDefaultAsync(m => m.DocumentId == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Documents == null)
            {
                return Problem("Entity set 'dbwarehouseContext.Documents'  is null.");
            }
            var document = await _context.Documents.FindAsync(id);
            if (document != null)
            {
                _context.Documents.Remove(document);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(int id)
        {
          return _context.Documents.Any(e => e.DocumentId == id);
        }
    }
}
