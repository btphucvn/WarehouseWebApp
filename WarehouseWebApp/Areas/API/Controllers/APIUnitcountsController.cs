using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration;
using WarehouseWebApp.Extension;
using WarehouseWebApp.Models;
using WarehouseWebApp.Variable;

namespace WarehouseWebApp.Areas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIUnitcountsController : ControllerBase
    {
        private readonly dbwarehouseContext _context;

        public APIUnitcountsController(dbwarehouseContext context)
        {
            _context = context;
        }

        // GET: api/APIUnitcounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Unitcount>>> GetUnitcounts(int? page, string? search)
        {
            var lsUnitcount = _context.Unitcounts.AsQueryable();
            int totalPage = NumberExtension.RoundUpPage(lsUnitcount.Count());

            if (!string.IsNullOrEmpty(search))
            {
                lsUnitcount = lsUnitcount.Where(oh => oh.Name.Contains(search));
            }
            int tmpPage = 1;
            tmpPage = page ?? default(int);
            if (page.HasValue)
            {
                lsUnitcount = lsUnitcount.Skip((tmpPage - 1) * CommonVariables.PAGE_SIZE).Take(CommonVariables.PAGE_SIZE);
            }
            else
            {
                lsUnitcount = lsUnitcount.Skip(0).Take(CommonVariables.PAGE_SIZE);
            }

            var result = Ok(new
            {
                error = "",
                result =new
                {
                    currentPage = tmpPage,
                    numberOfPage = totalPage,
                    contents =lsUnitcount
                }
            });
            return result;
        }

        //// GET: api/APIUnitcounts/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Unitcount>> GetUnitcount(int id)
        //{
        //    var unitcount = await _context.Unitcounts.FindAsync(id);

        //    if (unitcount == null)
        //    {
        //        return NotFound();
        //    }

        //    return unitcount;
        //}

        // PUT: api/APIUnitcounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnitcount(int id, Unitcount unitcount)
        {
            if (id != unitcount.UnitCountId)
            {
                return BadRequest();
            }

            _context.Entry(unitcount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UnitcountExists(id))
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

        // POST: api/APIUnitcounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Unitcount>> PostUnitcount(Unitcount unitcount)
        {
            _context.Unitcounts.Add(unitcount);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUnitcount", new { id = unitcount.UnitCountId }, unitcount);
        }

        // DELETE: api/APIUnitcounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnitcount(int id)
        {
            var unitcount = await _context.Unitcounts.FindAsync(id);
            if (unitcount == null)
            {
                return NotFound();
            }

            _context.Unitcounts.Remove(unitcount);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UnitcountExists(int id)
        {
            return _context.Unitcounts.Any(e => e.UnitCountId == id);
        }
    }
}
