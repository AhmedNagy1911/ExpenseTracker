using ExpenseTracker.Application.Contracts.RecurringTransactions;
using ExpenseTracker.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RecurringTransactionsController(IRecurringTransactionService recurringTransactionService) : ControllerBase
{
    private readonly IRecurringTransactionService _recurringTransactionService = recurringTransactionService;

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] RecurringTransactionFilters filters, CancellationToken cancellationToken)
    {
        var recurringTransactions = await _recurringTransactionService.GetAllAsync(UserId, filters, cancellationToken);
        return Ok(recurringTransactions);
    }


}
