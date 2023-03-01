using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseWebApp.Extension;
using WarehouseWebApp.Models;
using WarehouseWebApp.Variable;

namespace WarehouseWebApp.Areas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APISuppliersController : ControllerBase
    {
        private readonly dbwarehouseContext _context;

        public APISuppliersController(dbwarehouseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Unit>>> GetSuppliers(int? page, string? search)
        {
            var ls = _context.Suppliers.AsQueryable();
            int totalPage = NumberExtension.RoundUpPage(ls.Count());

            if (!string.IsNullOrEmpty(search))
            {
                ls = ls.Where(oh => oh.SupplierName.Contains(search));
            }
            int tmpPage = 1;
            tmpPage = page ?? default(int);
            if (page.HasValue)
            {
                ls = ls.Skip((tmpPage - 1) * CommonVariables.PAGE_SIZE).Take(CommonVariables.PAGE_SIZE);
            }
            else
            {
                ls = ls.Skip(0).Take(CommonVariables.PAGE_SIZE);
            }

            var result = Ok(new
            {
                error = "",
                result = new
                {
                    currentPage = tmpPage,
                    numberOfPage = totalPage,
                    contents = ls
                }
            });
            return result;
        }


        // GET: api/APISuppliers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplier(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return NotFound();
            }

            return supplier;
        }

        // PUT: api/APISuppliers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupplier(int id, Supplier supplier)
        {
            if (id != supplier.SupplierId)
            {
                return BadRequest();
            }

            _context.Entry(supplier).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/APISuppliers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Supplier>> PostSupplier(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSupplier", new { id = supplier.SupplierId }, supplier);
        }

        // DELETE: api/APISuppliers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.SupplierId == id);
        }
    }
}
