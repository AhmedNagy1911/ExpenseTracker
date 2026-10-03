using ExpenseTracker.API.Extensions;
using ExpenseTracker.Application.Contracts.Budgets;
using ExpenseTracker.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BudgetsController(IBudgetService budgetService) : ControllerBase
{
    private readonly IBudgetService _budgetService = budgetService;

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] BudgetFilters filters, CancellationToken cancellationToken)
    {
        var budgets = await _budgetService.GetAllAsync(UserId, filters, cancellationToken);
        return Ok(budgets);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _budgetService.GetAsync(UserId, id, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] BudgetRequest request, CancellationToken cancellationToken)
    {
        var result = await _budgetService.AddAsync(UserId, request, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] BudgetRequest request, CancellationToken cancellationToken)
    {
        var result = await _budgetService.UpdateAsync(UserId, id, request, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _budgetService.DeleteAsync(UserId, id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
