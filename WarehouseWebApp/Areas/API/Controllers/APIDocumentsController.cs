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
    public class APIDocumentsController : ControllerBase
    {
        private readonly dbwarehouseContext _context;

        public APIDocumentsController(dbwarehouseContext context)
        {
            _context = context;
        }

        // GET: api/APIDocuments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocuments(int? page, string? search,int documenttype)
        {
            var ls = _context.Documents.Include(x => x.Unit).Include(x=>x.Supplier).Where(x=>x.DocumentType == documenttype).AsQueryable();

            int totalPage = NumberExtension.RoundUpPage(ls.Count());

            if (!string.IsNullOrEmpty(search))
            {
                ls = ls.Where(oh => oh.DocumentCode.Contains(search));
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

        // GET: api/APIDocuments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Document>> GetDocument(int id)
        {
            var document = await _context.Documents.FindAsync(id);

            if (document == null)
            {
                return NotFound();
            }

            return document;
        }

  
    }
}
