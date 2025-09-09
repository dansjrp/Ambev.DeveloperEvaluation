using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser;

public class GetUsersCommand : IRequest<GetUsersResult>
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string? Order { get; set; }
}
