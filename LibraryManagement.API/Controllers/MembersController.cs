using LibraryManagement.Application.Members.Commands.CreateMember;
using LibraryManagement.Application.Members.Commands.DeleteMember;
using LibraryManagement.Application.Members.Commands.UpdateMember;
using LibraryManagement.Application.Members.Dtos;
using LibraryManagement.Application.Members.Queries.GetMemberById;
using LibraryManagement.Application.Members.Queries.GetMembers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    private readonly IMediator _mediator;

    public MembersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> GetMembers([FromQuery] MemberFilter filter, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMembersQuery(filter), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetMemberById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMemberByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> CreateMember([FromBody] CreateMemberDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateMemberCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetMemberById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateMember(Guid id, [FromBody] UpdateMemberDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateMemberCommand(id, dto), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteMember(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteMemberCommand(id), cancellationToken);
        return NoContent();
    }
}
