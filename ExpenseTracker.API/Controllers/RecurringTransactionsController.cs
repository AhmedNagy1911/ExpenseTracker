using ExpenseTracker.API.Extensions;
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

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _recurringTransactionService.GetAsync(UserId, id, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] RecurringTransactionRequest request, CancellationToken cancellationToken)
    {
        var result = await _recurringTransactionService.AddAsync(UserId, request, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] RecurringTransactionRequest request, CancellationToken cancellationToken)
    {
        var result = await _recurringTransactionService.UpdateAsync(UserId, id, request, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPut("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _recurringTransactionService.ToggleStatusAsync(UserId, id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _recurringTransactionService.DeleteAsync(UserId, id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
