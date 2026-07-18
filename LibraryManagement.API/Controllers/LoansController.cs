using LibraryManagement.Application.Loans.Commands.CreateLoan;
using LibraryManagement.Application.Loans.Commands.ReturnLoan;
using LibraryManagement.Application.Loans.Dtos;
using LibraryManagement.Application.Loans.Queries.GetLoanById;
using LibraryManagement.Application.Loans.Queries.GetLoans;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly IMediator _mediator;

    public LoansController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> GetLoans([FromQuery] LoanFilter filter, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLoansQuery(filter), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetLoanById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLoanByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> CreateLoan([FromBody] CreateLoanDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateLoanCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetLoanById), new { id = result.Id }, result);
    }

    [HttpPost("{id:guid}/return")]
    public async Task<ActionResult> ReturnLoan(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ReturnLoanCommand(id), cancellationToken);
        return NoContent();
    }
}
