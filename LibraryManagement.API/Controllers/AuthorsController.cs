using LibraryManagement.Application.Authors.Commands.CreateAuthor;
using LibraryManagement.Application.Authors.Commands.DeleteAuthor;
using LibraryManagement.Application.Authors.Commands.UpdateAuthor;
using LibraryManagement.Application.Authors.Dtos;
using LibraryManagement.Application.Authors.Queries.GetAuthorById;
using LibraryManagement.Application.Authors.Queries.GetAuthors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> GetAuthors([FromQuery] AuthorFilter filter, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAuthorsQuery(filter), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetAuthorById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAuthorByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAuthor([FromBody] CreateAuthorDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateAuthorCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetAuthorById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateAuthor(Guid id, [FromBody] UpdateAuthorDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateAuthorCommand(id, dto), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteAuthor(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteAuthorCommand(id), cancellationToken);
        return NoContent();
    }
}
