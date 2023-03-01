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
    public class APIDocumentdetailsController : ControllerBase
    {
        private readonly dbwarehouseContext _context;

        public APIDocumentdetailsController(dbwarehouseContext context)
        {
            _context = context;
        }

        // GET: api/APIDocumentdetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Documentdetail>>> GetDocumentdetails(int? page, int documentid)
        {
            var ls = _context.Documentdetails.Include(x=>x.BarcodeNavigation).Where(x=>x.DocumentId== documentid).AsQueryable();
            int totalPage = NumberExtension.RoundUpPage(ls.Count());

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



    }
}
