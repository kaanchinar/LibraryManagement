using LibraryManagement.Application.Publishers.Commands.CreatePublisher;
using LibraryManagement.Application.Publishers.Commands.DeletePublisher;
using LibraryManagement.Application.Publishers.Commands.UpdatePublisher;
using LibraryManagement.Application.Publishers.Dtos;
using LibraryManagement.Application.Publishers.Queries.GetPublisherById;
using LibraryManagement.Application.Publishers.Queries.GetPublishers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

/// <summary>
/// Manages publisher resources including creation, retrieval, updating, and deletion.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PublishersController : ControllerBase
{
    private readonly IMediator _mediator;

    public PublishersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves a paginated list of publishers with optional filtering.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult> GetPublishers([FromQuery] PublisherFilter filter, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPublishersQuery(filter), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a single publisher by its unique identifier.
    /// </summary>
    /// <param name="id">The publisher's unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="404">The publisher was not found.</response>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetPublisherById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPublisherByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new publisher.
    /// </summary>
    /// <param name="dto">The publisher details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="201">The publisher was created successfully.</response>
    [Authorize(Roles = "Admin,Librarian")]
    [HttpPost]
    public async Task<ActionResult> CreatePublisher([FromBody] CreatePublisherDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreatePublisherCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetPublisherById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates an existing publisher.
    /// </summary>
    /// <param name="id">The publisher's unique identifier.</param>
    /// <param name="dto">The updated publisher details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="404">The publisher was not found.</response>
    [Authorize(Roles = "Admin,Librarian")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdatePublisher(Guid id, [FromBody] UpdatePublisherDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdatePublisherCommand(id, dto), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a publisher. Fails if the publisher still has associated books.
    /// </summary>
    /// <param name="id">The publisher's unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="204">The publisher was deleted successfully.</response>
    /// <response code="400">The publisher has associated books and cannot be deleted.</response>
    /// <response code="404">The publisher was not found.</response>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeletePublisher(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeletePublisherCommand(id), cancellationToken);
        return NoContent();
    }
}
