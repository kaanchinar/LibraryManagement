using LibraryManagement.Application.Books.Commands.CreateBook;
using LibraryManagement.Application.Books.Commands.DeleteBook;
using LibraryManagement.Application.Books.Commands.UpdateBook;
using LibraryManagement.Application.Books.Dtos;
using LibraryManagement.Application.Books.Queries.GetBookById;
using LibraryManagement.Application.Books.Queries.GetBooks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> GetBooks([FromQuery] BookFilter filter, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBooksQuery(filter), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetBookById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBookByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> CreateBook([FromBody] CreateBookDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateBookCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetBookById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateBook(Guid id, [FromBody] UpdateBookDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateBookCommand(id, dto), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteBook(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteBookCommand(id), cancellationToken);
        return NoContent();
    }
}
