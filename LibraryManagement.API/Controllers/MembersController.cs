using LibraryManagement.Application.Members.Commands.CreateMember;
using LibraryManagement.Application.Members.Commands.DeleteMember;
using LibraryManagement.Application.Members.Commands.UpdateMember;
using LibraryManagement.Application.Members.Dtos;
using LibraryManagement.Application.Members.Queries.GetMemberById;
using LibraryManagement.Application.Members.Queries.GetMembers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

/// <summary>
/// Manages library member resources including creation, retrieval, updating, and deletion.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    private readonly IMediator _mediator;

    public MembersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves a paginated list of members with optional filtering.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult> GetMembers([FromQuery] MemberFilter filter, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMembersQuery(filter), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a single member by their unique identifier.
    /// </summary>
    /// <param name="id">The member's unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="404">The member was not found.</response>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetMemberById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMemberByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Registers a new library member.
    /// </summary>
    /// <param name="dto">The member details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="201">The member was created successfully.</response>
    [HttpPost]
    public async Task<ActionResult> CreateMember([FromBody] CreateMemberDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateMemberCommand(dto), cancellationToken);
        return CreatedAtAction(nameof(GetMemberById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates an existing member.
    /// </summary>
    /// <param name="id">The member's unique identifier.</param>
    /// <param name="dto">The updated member details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="404">The member was not found.</response>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateMember(Guid id, [FromBody] UpdateMemberDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateMemberCommand(id, dto), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a member. Fails if the member still has active or historical loans.
    /// </summary>
    /// <param name="id">The member's unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="204">The member was deleted successfully.</response>
    /// <response code="400">The member has associated loans and cannot be deleted.</response>
    /// <response code="404">The member was not found.</response>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteMember(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteMemberCommand(id), cancellationToken);
        return NoContent();
    }
}
