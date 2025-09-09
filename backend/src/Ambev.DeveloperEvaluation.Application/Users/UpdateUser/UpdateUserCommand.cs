using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;
using System;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

/// <summary>
/// Command para atualizar um usuário existente.
/// </summary>
public class UpdateUserCommand : IRequest<User>
{
    public Guid Id { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public Address Address { get; set; } = new Address();
    // Adicione outros campos conforme necessário
}
