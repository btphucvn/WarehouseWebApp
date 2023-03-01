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
    public class APIGroupgoodsController : ControllerBase
    {
        private readonly dbwarehouseContext _context;

        public APIGroupgoodsController(dbwarehouseContext context)
        {
            _context = context;
        }

        // GET: api/APIGroupgoods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Groupgood>>> GetGroupgoods(int? page, string? search)
        {
            var ls = _context.Groupgoods.AsQueryable();
            int totalPage = NumberExtension.RoundUpPage(ls.Count());

            if (!string.IsNullOrEmpty(search))
            {
                ls = ls.Where(oh => oh.GroupName.Contains(search));
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

        // GET: api/APIGroupgoods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Groupgood>> GetGroupgood(int id)
        {
            var groupgood = await _context.Groupgoods.FindAsync(id);

            if (groupgood == null)
            {
                return NotFound();
            }

            return groupgood;
        }

        // PUT: api/APIGroupgoods/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupgood(int id, Groupgood groupgood)
        {
            if (id != groupgood.GroupGoodId)
            {
                return BadRequest();
            }

            _context.Entry(groupgood).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupgoodExists(id))
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

        // POST: api/APIGroupgoods
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Groupgood>> PostGroupgood(Groupgood groupgood)
        {
            _context.Groupgoods.Add(groupgood);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroupgood", new { id = groupgood.GroupGoodId }, groupgood);
        }

        // DELETE: api/APIGroupgoods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupgood(int id)
        {
            var groupgood = await _context.Groupgoods.FindAsync(id);
            if (groupgood == null)
            {
                return NotFound();
            }

            _context.Groupgoods.Remove(groupgood);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GroupgoodExists(int id)
        {
            return _context.Groupgoods.Any(e => e.GroupGoodId == id);
        }
    }
}
