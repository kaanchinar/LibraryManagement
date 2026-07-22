using LibraryManagement.Application.Authors.Commands.CreateAuthor;
using LibraryManagement.Application.Authors.Commands.DeleteAuthor;
using LibraryManagement.Application.Authors.Commands.UpdateAuthor;
using LibraryManagement.Application.Authors.Dtos;
using LibraryManagement.Application.Authors.Queries.GetAuthorById;
using LibraryManagement.Application.Authors.Queries.GetAuthors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

/// <summary>
/// Manages author resources including creation, retrieval, updating, and deletion.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves a paginated list of authors with optional filtering.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult> GetAuthors([FromQuery] AuthorFilter filter, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAuthorsQuery(filter), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a single author by their unique identifier.
    /// </summary>
    /// <param name="id">The author's unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="404">The author was not found.</response>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetAuthorById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAuthorByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new author.
    /// </summary>
    /// <param name="dto">The author details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="201">The author was created successfully.</response>
    [HttpPost]
    public async Task<ActionResult> CreateAuthor([FromBody] CreateAuthorDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateAuthorCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetAuthorById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates an existing author.
    /// </summary>
    /// <param name="id">The author's unique identifier.</param>
    /// <param name="dto">The updated author details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="404">The author was not found.</response>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateAuthor(Guid id, [FromBody] UpdateAuthorDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateAuthorCommand(id, dto), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes an author. Fails if the author still has books.
    /// </summary>
    /// <param name="id">The author's unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="204">The author was deleted successfully.</response>
    /// <response code="400">The author has associated books and cannot be deleted.</response>
    /// <response code="404">The author was not found.</response>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteAuthor(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteAuthorCommand(id), cancellationToken);
        return NoContent();
    }
}
