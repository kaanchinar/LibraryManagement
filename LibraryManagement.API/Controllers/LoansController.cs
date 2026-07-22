using LibraryManagement.Application.Loans.Commands.CreateLoan;
using LibraryManagement.Application.Loans.Commands.ReturnLoan;
using LibraryManagement.Application.Loans.Dtos;
using LibraryManagement.Application.Loans.Queries.GetLoanById;
using LibraryManagement.Application.Loans.Queries.GetLoans;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

/// <summary>
/// Manages book loan operations including checkout and return.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoansController : ControllerBase
{
    private readonly IMediator _mediator;

    public LoansController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves a paginated list of loans with optional filtering.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult> GetLoans([FromQuery] LoanFilter filter, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLoansQuery(filter), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a single loan by its unique identifier.
    /// </summary>
    /// <param name="id">The loan's unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="404">The loan was not found.</response>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetLoanById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLoanByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Checks out a book to a member, creating a new loan. Decrements the book's available copies.
    /// </summary>
    /// <param name="dto">The loan details including book and member IDs.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="201">The loan was created successfully.</response>
    /// <response code="400">The book has no available copies or the member is inactive.</response>
    [Authorize(Roles = "Admin,Librarian")]
    [HttpPost]
    public async Task<ActionResult> CreateLoan([FromBody] CreateLoanDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateLoanCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetLoanById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Returns a previously checked-out book, marking the loan as returned. Increments the book's available copies.
    /// </summary>
    /// <param name="id">The loan's unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="204">The loan was returned successfully.</response>
    /// <response code="400">The loan has already been returned.</response>
    /// <response code="404">The loan was not found.</response>
    [Authorize(Roles = "Admin,Librarian")]
    [HttpPost("{id:guid}/return")]
    public async Task<ActionResult> ReturnLoan(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ReturnLoanCommand(id), cancellationToken);
        return NoContent();
    }
}
