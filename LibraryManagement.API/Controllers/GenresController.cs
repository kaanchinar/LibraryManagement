using LibraryManagement.Application.Genres.Commands.CreateGenre;
using LibraryManagement.Application.Genres.Commands.DeleteGenre;
using LibraryManagement.Application.Genres.Commands.UpdateGenre;
using LibraryManagement.Application.Genres.Dtos;
using LibraryManagement.Application.Genres.Queries.GetGenreById;
using LibraryManagement.Application.Genres.Queries.GetGenres;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

/// <summary>
/// Manages genre resources including creation, retrieval, updating, and deletion.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GenresController : ControllerBase
{
    private readonly IMediator _mediator;

    public GenresController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves a paginated list of genres with optional filtering.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult> GetGenres([FromQuery] GenreFilter filter, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetGenresQuery(filter), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a single genre by its unique identifier.
    /// </summary>
    /// <param name="id">The genre's unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="404">The genre was not found.</response>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetGenreById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetGenreByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new genre.
    /// </summary>
    /// <param name="dto">The genre details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="201">The genre was created successfully.</response>
    [Authorize(Roles = "Admin,Librarian")]
    [HttpPost]
    public async Task<ActionResult> CreateGenre([FromBody] CreateGenreDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateGenreCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetGenreById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates an existing genre.
    /// </summary>
    /// <param name="id">The genre's unique identifier.</param>
    /// <param name="dto">The updated genre details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="404">The genre was not found.</response>
    [Authorize(Roles = "Admin,Librarian")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateGenre(Guid id, [FromBody] UpdateGenreDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateGenreCommand(id, dto), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a genre. Fails if the genre still has associated books.
    /// </summary>
    /// <param name="id">The genre's unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="204">The genre was deleted successfully.</response>
    /// <response code="400">The genre has associated books and cannot be deleted.</response>
    /// <response code="404">The genre was not found.</response>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteGenre(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteGenreCommand(id), cancellationToken);
        return NoContent();
    }
}
