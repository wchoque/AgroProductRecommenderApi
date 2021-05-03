using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using AppCentroIdiomas.Models;

namespace AppCentroIdiomas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly AppCentroEstudiosDBContext _context;

        public InvoiceController(AppCentroEstudiosDBContext context)
        {
            _context = context;
        }

        // GET: api/Invoice
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
            return await _context.Invoices.ToListAsync();
        }

        // GET: api/Invoice
        [HttpGet("GetInvoiceByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<AvailableInvoices>>> GetInvoiceByUserId(int userId)
        {
            var _availableInvoices = new AvailableInvoices();
            var invoicesByUser = await _context.Invoices
                                    .Include(x => x.CourseBySemesterEnroll.UserByTypeStudent)
                                    .Where(x => x.CourseBySemesterEnroll.UserByTypeStudent.UserId == userId)
                                    .ToListAsync();

            foreach (var item in invoicesByUser)
            {
                var _newAvailableInvoice = new AvailableInvoice {
                    Id = item.Id,
                    Description = item.Description,
                    Semester = "2021-1",
                    Amount = item.Amount,
                    Deadline = item.Deadline.ToString("yyyy/MM/dd HH:mm:ss"),
                    PaidAt = item.PaidAt?.ToString("yyyy/MM/dd HH:mm:ss"),
                    IsPaid = item.PaidAt != null
                };
                _availableInvoices.AvailableInvoicesList.Add(_newAvailableInvoice);
            }
            return Ok(new { invoices = _availableInvoices.AvailableInvoicesList });
        }


        // GET: api/Invoice/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);

            if (invoice == null)
            {
                return NotFound();
            }

            return invoice;
        }

        // PUT: api/Invoice/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoice(int id, Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return BadRequest();
            }

            _context.Entry(invoice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
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

        // POST: api/Invoice
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInvoice", new { id = invoice.Id }, invoice);
        }

        // DELETE: api/Invoice/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Invoice>> DeleteInvoice(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();

            return invoice;
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }
    }
}
