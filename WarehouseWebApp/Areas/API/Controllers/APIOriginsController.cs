using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WarehouseWebApp.Extension;
using WarehouseWebApp.Models;
using WarehouseWebApp.Variable;

namespace WarehouseWebApp.Areas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIOriginsController : ControllerBase
    {
        private readonly dbwarehouseContext _context;

        public APIOriginsController(dbwarehouseContext context)
        {
            _context = context;
        }

        // GET: api/APIOrigins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Origin>>> GetOrigins(int? page, string? search)
        {
            var ls = _context.Origins.AsQueryable();
            int totalPage = NumberExtension.RoundUpPage(ls.Count());

            if (!string.IsNullOrEmpty(search))
            {
                ls = ls.Where(oh => oh.OriginName.Contains(search));
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

        // GET: api/APIOrigins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Origin>> GetOrigin(int id)
        {
            var origin = await _context.Origins.FindAsync(id);

            if (origin == null)
            {
                return NotFound();
            }

            return origin;
        }

        // PUT: api/APIOrigins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrigin(int id, Origin origin)
        {
            if (id != origin.OriginId)
            {
                return BadRequest();
            }

            _context.Entry(origin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OriginExists(id))
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

        // POST: api/APIOrigins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Origin>> PostOrigin(Origin origin)
        {
            _context.Origins.Add(origin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrigin", new { id = origin.OriginId }, origin);
        }

        // DELETE: api/APIOrigins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrigin(int id)
        {
            var origin = await _context.Origins.FindAsync(id);
            if (origin == null)
            {
                return NotFound();
            }

            _context.Origins.Remove(origin);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OriginExists(int id)
        {
            return _context.Origins.Any(e => e.OriginId == id);
        }
    }
}
