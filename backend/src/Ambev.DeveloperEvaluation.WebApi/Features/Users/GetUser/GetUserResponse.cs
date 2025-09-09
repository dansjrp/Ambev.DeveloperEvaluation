using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;

/// <summary>
/// API response model for GetUser operation
/// </summary>
public class GetUserResponse
{
    /// <summary>
    /// The user's first name
    /// </summary>
    public string Firstname { get; set; } = string.Empty;

    /// <summary>
    /// The user's last name
    /// </summary>
    public string Lastname { get; set; } = string.Empty;

    /// <summary>
    /// The user's address
    /// </summary>
    public Ambev.DeveloperEvaluation.Domain.Entities.Address Address { get; set; } = new Ambev.DeveloperEvaluation.Domain.Entities.Address();

    /// <summary>
    /// The unique identifier of the user
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The user's full name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The user's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The user's phone number
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// The user's role in the system
    /// </summary>
    public UserRole Role { get; set; }

    /// <summary>
    /// The current status of the user
    /// </summary>
    public UserStatus Status { get; set; }
}
