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
    public class APIGoodsController : ControllerBase
    {
        private readonly dbwarehouseContext _context;

        public APIGoodsController(dbwarehouseContext context)
        {
            _context = context;
        }

        // GET: api/APIGoods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Good>>> GetGoods(int? page, string? search)
        {
            var ls = _context.Goods.Include(x=>x.Supplier).Include(x=>x.GroupGood).Include(x=>x.Unit).Include(x=>x.Origin).AsQueryable();
            int totalPage = NumberExtension.RoundUpPage(ls.Count());

            if (!string.IsNullOrEmpty(search))
            {
                ls = ls.Where(oh => oh.CategoryName.Contains(search));
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

        // GET: api/APIUnits/5
        [HttpGet("{barcode}")]
        public async Task<ActionResult<Good>> GetGood(string barcode)
        {
            var result = await _context.Goods.Include(x => x.Supplier).Include(x => x.GroupGood).Include(x => x.Unit).Include(x => x.Origin).FirstAsync(x=>x.Barcode==barcode);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

    }
}
