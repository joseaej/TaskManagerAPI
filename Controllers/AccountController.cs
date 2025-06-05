using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlowAPI.Services;
using TasksManagerAPI.Data;
using TasksManagerAPI.Models.Entity;

namespace TasksManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public AccountController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: api/Account/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Account>> GetAccountById(int id)
        {
            
            var account = await _dbContext.Accounts.FindAsync(id);
            if (account == null)
                return NotFound();

            return Ok(account);
        }

        // POST: api/Account
        [HttpPost]
        public async Task<ActionResult<Account>> CreateAccount([FromBody] Account account)
        {
            if (account is null ||
                string.IsNullOrWhiteSpace(account.Email) ||
                string.IsNullOrWhiteSpace(account.Username) ||
                string.IsNullOrWhiteSpace(account.PasswordHash))
            {
                return BadRequest("All fields are required.");
            }

            account.PasswordHash = CryptographyService.EncryptPassword(account.PasswordHash);

            await _dbContext.Accounts.AddAsync(account);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccountById), new { id = account.Id }, account);
        }

        // PUT: api/Account/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateAccount(int id, [FromBody] Account updatedAccount)
        {
            if (updatedAccount is null ||
                updatedAccount.Id != id ||
                string.IsNullOrWhiteSpace(updatedAccount.Email) ||
                string.IsNullOrWhiteSpace(updatedAccount.Username) ||
                string.IsNullOrWhiteSpace(updatedAccount.PasswordHash))
            {
                return BadRequest("Invalid data.");
            }

            var existingAccount = await _dbContext.Accounts.FindAsync(id);
            if (existingAccount == null)
                return NotFound();

            existingAccount.Email = updatedAccount.Email;
            existingAccount.Username = updatedAccount.Username;
            existingAccount.PasswordHash = CryptographyService.EncryptPassword(updatedAccount.PasswordHash);

            _dbContext.Accounts.Update(existingAccount);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
