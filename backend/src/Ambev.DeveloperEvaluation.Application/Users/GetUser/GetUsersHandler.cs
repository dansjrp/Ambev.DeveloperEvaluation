using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser;

public class GetUsersHandler : IRequestHandler<GetUsersCommand, GetUsersResult>
{
    private readonly Domain.Repositories.IUserRepository _userRepository;

    public GetUsersHandler(Domain.Repositories.IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<GetUsersResult> Handle(GetUsersCommand request, CancellationToken cancellationToken)
    {
        var (users, totalCount) = await _userRepository.GetPaginatedAsync(request.Page, request.Size, request.Order, cancellationToken);

        var result = new GetUsersResult
        {
            Users = users.Select(u => new GetUserResult
            {
                Id = u.Id,
                Username = u.Username,
                Password = u.Password,
                Name = new Domain.Entities.Name {
                    Firstname = u.Name?.Firstname ?? string.Empty,
                    Lastname = u.Name?.Lastname ?? string.Empty
                },
                Address = u.Address,
                Email = u.Email,
                Phone = u.Phone,
                Role = u.Role,
                Status = u.Status
            }),
            CurrentPage = request.Page,
            TotalPages = totalCount == 0 ? 1 : (int)System.Math.Ceiling(totalCount / (double)request.Size),
            TotalCount = totalCount
        };

        return result;
    }
}
