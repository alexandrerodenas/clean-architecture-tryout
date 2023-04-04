using System.Net;
using Application.Expenses.Commands.CreateExpense;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebUI;

[ApiController]
public class CreateExpenseForUserController : ControllerBase
{
    private readonly IMediator _mediator;

    public CreateExpenseForUserController(
        IMediator mediator
    )
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("users/{userId}/expenses")]
    public async Task<ActionResult<Guid>> CreateExpenseFor(
        [FromRoute] Guid userId,
        [FromBody] CreateExpenseCommand createExpenseCommand
    )
    {
        if (userId != createExpenseCommand.UserId)
        {
            return BadRequest("User id inconsistent.");
        }
        return await _mediator.Send(createExpenseCommand);
    }
}
