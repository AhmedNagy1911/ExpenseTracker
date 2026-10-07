using ExpenseTracker.Application.Contracts.Reports;
using ExpenseTracker.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.API.Controllers;

[Route("api/reports")]
[ApiController]
[Authorize]
public class ReportsController(IReportService reportService) : ControllerBase
{
    private readonly IReportService _reportService = reportService;

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet("monthly-summary")]
    public async Task<IActionResult> GetMonthlySummary([FromQuery] int year, [FromQuery] int month, CancellationToken cancellationToken)
    {
        if (month is < 1 or > 12)
            return BadRequest("Month must be between 1 and 12.");

        var summary = await _reportService.GetMonthlySummaryAsync(UserId, year, month, cancellationToken);
        return Ok(summary);
    }


    [HttpGet("category-breakdown")]
    public async Task<IActionResult> GetCategoryBreakdown([FromQuery] int year, [FromQuery] int month, CancellationToken cancellationToken)
    {
        if (month is < 1 or > 12)
            return BadRequest("Month must be between 1 and 12.");

        var breakdown = await _reportService.GetCategoryBreakdownAsync(UserId, year, month, cancellationToken);
        return Ok(breakdown);
    }


    [HttpGet("month-comparison")]
    public async Task<IActionResult> GetMonthComparison([FromQuery] MonthComparisonFilters filters, CancellationToken cancellationToken)
    {
        var comparison = await _reportService.GetMonthComparisonAsync(UserId, filters, cancellationToken);
        return Ok(comparison);
    }


}
