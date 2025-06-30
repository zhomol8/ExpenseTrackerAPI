using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TransactionsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("user-transactions")]
    public async Task<IActionResult> GetUserTransactions()
    {
        // Get the authenticated user's ID from JWT claims
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return Unauthorized();

        if (!int.TryParse(userId, out int userIdInt))
            return Unauthorized();

        var transactions = await _context.Transactions
            .Where(t => t.UserId == userIdInt)
            .ToListAsync();

        return Ok(transactions);
    }
}
