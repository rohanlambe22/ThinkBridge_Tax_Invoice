
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BuggyApp.Data;

namespace BuggyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly AppDbContext _db;

        public InvoiceController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetInvoice()
        {
            var invoice = _db.Invoices
                .Include(i => i.Items)
                .OrderBy(i => i.InvoiceID)
                .FirstOrDefault();

            if (invoice == null)
            {
                return NotFound("No invoice found");
            }

            var items = invoice.Items.Select(ii => new { name = ii.Name, price = ii.Price });
            return Ok(new { items });
        }
    }
}
