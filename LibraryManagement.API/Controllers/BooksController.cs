using LibraryManagement.Application.Books.Commands.CreateBook;
using LibraryManagement.Application.Books.Commands.DeleteBook;
using LibraryManagement.Application.Books.Commands.UpdateBook;
using LibraryManagement.Application.Books.Dtos;
using LibraryManagement.Application.Books.Queries.GetBookById;
using LibraryManagement.Application.Books.Queries.GetBooks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

/// <summary>
/// Manages book resources including creation, retrieval, updating, and deletion.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves a paginated list of books with optional filtering.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult> GetBooks([FromQuery] BookFilter filter, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBooksQuery(filter), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a single book by its unique identifier.
    /// </summary>
    /// <param name="id">The book's unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="404">The book was not found.</response>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetBookById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBookByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new book.
    /// </summary>
    /// <param name="dto">The book details including author, and optionally genre and publisher.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="201">The book was created successfully.</response>
    [HttpPost]
    public async Task<ActionResult> CreateBook([FromBody] CreateBookDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateBookCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetBookById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates an existing book.
    /// </summary>
    /// <param name="id">The book's unique identifier.</param>
    /// <param name="dto">The updated book details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="404">The book was not found.</response>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateBook(Guid id, [FromBody] UpdateBookDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateBookCommand(id, dto), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a book. Fails if the book still has active or historical loans.
    /// </summary>
    /// <param name="id">The book's unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="204">The book was deleted successfully.</response>
    /// <response code="400">The book has associated loans and cannot be deleted.</response>
    /// <response code="404">The book was not found.</response>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteBook(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteBookCommand(id), cancellationToken);
        return NoContent();
    }
}
