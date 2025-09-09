using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

/// <summary>
/// Handler para atualizar um usuário existente.
/// </summary>
public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, User>
{
    private readonly IUserRepository _userRepository;
    private readonly UpdateUserValidator _validator;

    public UpdateUserHandler(IUserRepository userRepository, UpdateUserValidator validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }

    public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        var user = new User
        {
            Id = request.Id,
            Name = new Name(request.Firstname, request.Lastname),
            Email = request.Email,
            Phone = request.Phone,
            Role = Enum.TryParse<UserRole>(request.Role, true, out var role) ? role : UserRole.None,
            Status = Enum.TryParse<UserStatus>(request.Status, true, out var status) ? status : UserStatus.Unknown,
            Address = request.Address
            // Adicione outros campos conforme necessário
        };
        return await _userRepository.UpdateAsync(user, cancellationToken);
    }
}
