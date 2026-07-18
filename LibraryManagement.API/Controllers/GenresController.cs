using LibraryManagement.Application.Genres.Commands.CreateGenre;
using LibraryManagement.Application.Genres.Commands.DeleteGenre;
using LibraryManagement.Application.Genres.Commands.UpdateGenre;
using LibraryManagement.Application.Genres.Dtos;
using LibraryManagement.Application.Genres.Queries.GetGenreById;
using LibraryManagement.Application.Genres.Queries.GetGenres;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenresController : ControllerBase
{
    private readonly IMediator _mediator;

    public GenresController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> GetGenres([FromQuery] GenreFilter filter, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetGenresQuery(filter), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetGenreById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetGenreByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> CreateGenre([FromBody] CreateGenreDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateGenreCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetGenreById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateGenre(Guid id, [FromBody] UpdateGenreDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateGenreCommand(id, dto), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteGenre(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteGenreCommand(id), cancellationToken);
        return NoContent();
    }
}
