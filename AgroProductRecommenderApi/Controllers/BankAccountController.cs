using System.Linq;
using System.Threading.Tasks;
using AgroProductRecommenderApi.Controllers.DTOs;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroProductRecommenderApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankAccountsController : ControllerBase
    {
        private readonly AgroProductRecommenderDBContext _context;

        public BankAccountsController(AgroProductRecommenderDBContext context)
        {
            _context = context;
        }

        // GET: api/BankAccounts
        [HttpGet]
        public IActionResult GetBankAccounts()
        {
            var accounts = _context.BankAccounts.Include(b => b.UserInformation).ToList();
            return Ok(accounts);
        }

        [HttpGet("GetBankAccounts/{userId}")]
        public IActionResult GetBankAccountsByUser(int userId)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return NotFound();

            var userInformation = _context.UserInformation.FirstOrDefault(x => x.Id == user.UserInformationId);
            if (userInformation == null)
                return NotFound();

            var accounts = _context.BankAccounts
                .Where(b => b.UserInformationId == userInformation.Id)
                .ToList();

            var bankAccounts = accounts.Select(x => new BankAccountDTO()
            {
                Id = x.Id,
                BankName = x.BankName,
                AccountType = x.AccountType,
                AccountNumber = x.AccountNumber,
                CCI = x.CCI
            });
           
            return Ok(bankAccounts);
        }

        // GET: api/BankAccounts/5
        [HttpGet("{id}")]
        public IActionResult GetBankAccount(int id)
        {
            var account = _context.BankAccounts.Find(id);
            if (account == null)
            {
                return NotFound();
            }

            var bankAccount = new BankAccountDTO
            {
                Id = account.Id,
                BankName = account.BankName,
                AccountType = account.AccountType,
                AccountNumber = account.AccountNumber,
                CCI = account.CCI
            };
            return Ok(bankAccount);
        }

        // POST: api/BankAccounts
        [HttpPost]
        public IActionResult PostBankAccount([FromBody] BankAccountDTO bankAccountModel)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Id == bankAccountModel.UserId);

            if (user == null)
                return NotFound();

            var userInformation = _context.UserInformation.FirstOrDefault(x => x.Id == user.UserInformationId);

            if (userInformation == null)
                return NotFound();

            var bankAccount = new BankAccount()
            {
                BankName = bankAccountModel.BankName,
                AccountType = bankAccountModel.AccountType,
                AccountNumber = bankAccountModel.AccountNumber,
                CCI = bankAccountModel.CCI,
                UserInformationId = userInformation.Id
            };
            _context.BankAccounts.Add(bankAccount);
            _context.SaveChanges();


            var bankAccountDto = new BankAccountDTO
            {
                Id = bankAccount.Id,
                BankName = bankAccount.BankName,
                AccountType = bankAccount.AccountType,
                AccountNumber = bankAccount.AccountNumber,
                CCI = bankAccount.CCI
            };

            return CreatedAtAction("GetBankAccount", new { id = bankAccount.Id }, bankAccountDto);
        }

        // PUT: api/BankAccounts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBankAccount(int id, [FromBody] BankAccountDTO bankAccount)
        {
            if (id != bankAccount.Id)
            {
                return BadRequest();
            }

            var existingBankAccount = await _context.BankAccounts.FindAsync(id);

            if (existingBankAccount == null)
                return NotFound();

            existingBankAccount.BankName = bankAccount.BankName;
            existingBankAccount.AccountType = bankAccount.AccountType;
            existingBankAccount.AccountNumber = bankAccount.AccountNumber;
            existingBankAccount.CCI = bankAccount.CCI;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/BankAccounts/5
        [HttpDelete("{id}")]
        public IActionResult DeleteBankAccount(int id)
        {
            var bankAccount = _context.BankAccounts.Find(id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            _context.BankAccounts.Remove(bankAccount);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
