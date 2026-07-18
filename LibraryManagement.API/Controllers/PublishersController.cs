using LibraryManagement.Application.Publishers.Commands.CreatePublisher;
using LibraryManagement.Application.Publishers.Commands.DeletePublisher;
using LibraryManagement.Application.Publishers.Commands.UpdatePublisher;
using LibraryManagement.Application.Publishers.Dtos;
using LibraryManagement.Application.Publishers.Queries.GetPublisherById;
using LibraryManagement.Application.Publishers.Queries.GetPublishers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublishersController : ControllerBase
{
    private readonly IMediator _mediator;

    public PublishersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> GetPublishers([FromQuery] PublisherFilter filter, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPublishersQuery(filter), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetPublisherById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPublisherByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> CreatePublisher([FromBody] CreatePublisherDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreatePublisherCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetPublisherById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdatePublisher(Guid id, [FromBody] UpdatePublisherDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdatePublisherCommand(id, dto), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeletePublisher(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeletePublisherCommand(id), cancellationToken);
        return NoContent();
    }
}
