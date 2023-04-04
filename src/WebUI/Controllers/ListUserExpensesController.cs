using Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebUI;

[ApiController]
public class ListUserExpensesController : ControllerBase
{

    private readonly IMediator _mediator;

    public ListUserExpensesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("users/{userId}/expenses")]
    public async Task<ActionResult<UserExpensesOutput>> ListExpensesByUser(
        [FromRoute] Guid userId,
        [FromQuery] OrderBy? orderBy,
        [FromQuery] SortBy? sortBy
    )
    {
        var listSortedExpensesQuery = new ListSortedExpensesQuery
        {
            UserId = userId,
            OrderBy = orderBy,
            SortBy = sortBy
        };
        return await _mediator.Send(listSortedExpensesQuery);
    }
}
