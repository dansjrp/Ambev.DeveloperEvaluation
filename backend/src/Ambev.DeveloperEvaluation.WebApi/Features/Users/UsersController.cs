using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.DeleteUser;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using Ambev.DeveloperEvaluation.Application.Users.UpdateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users;

/// <summary>
/// Controller for managing user operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UsersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all users paginated
    /// </summary>
    /// <param name="page">Page number</param>
    /// <param name="size">Number of items per page</param>
    /// <param name="order">Ordering of results</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paged list of users</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<GetUserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_size")] int size = 10,
        [FromQuery(Name = "_order")] string? order = null,
        CancellationToken cancellationToken = default)
    {
        var command = new GetUsersCommand
        {
            Page = page,
            Size = size,
            Order = order
        };

        var result = await _mediator.Send(command, cancellationToken);
        var mapped = result.Users.Select(u => _mapper.Map<GetUserResponse>(u)).ToList();

        return OkPaginated(new PaginatedList<GetUserResponse>(mapped, result.TotalCount, result.CurrentPage, result.TotalPages));
    }


    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <param name="request">The user creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created user details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateUserResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateUserCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<CreateUserResponse>
        {
            Success = true,
            Message = "User created successfully",
            Data = _mapper.Map<CreateUserResponse>(response)
        });
    }

    /// <summary>
    /// Retrieves a user by their ID
    /// </summary>
    /// <param name="id">The unique identifier of the user</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The user details if found</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetUserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser([FromRoute] Guid id, CancellationToken cancellationToken)
    {
    var command = _mapper.Map<GetUserCommand>(id);
        try
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (result == null)
                return ResourceNotFound("User not found", $"The user with ID {id} does not exist in our database");
            var userResponse = _mapper.Map<GetUserResponse>(result);
            return Ok(userResponse);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a user by their ID
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response if the user was deleted</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id, CancellationToken cancellationToken)
    {
    var command = _mapper.Map<DeleteUserCommand>(id);
        try
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (result == null)
                return ResourceNotFound("User not found", $"The user with ID {id} does not exist in our database");
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "User deleted successfully"
            });
        }
        catch (Exception ex)
        {
            return InternalServerError(ex.Message);
        }
    }

    /// <summary>
    /// Atualiza um usuário existente
    /// </summary>
    /// <param name="id">ID do usuário a ser atualizado</param>
    /// <param name="request">Dados para atualização</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Usuário atualizado</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetUserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return ValidationError("O id da rota não corresponde ao id do corpo da requisição.");

        var command = _mapper.Map<UpdateUserCommand>(request);
        try
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (result == null)
                return ResourceNotFound("User not found", $"The user with ID {id} does not exist in our database");
            var userResponse = _mapper.Map<GetUserResponse>(result);
            return Ok(new ApiResponseWithData<GetUserResponse>
            {
                Success = true,
                Message = "User updated successfully",
                Data = userResponse
            });
        }
        catch (Exception ex)
        {
            return InternalServerError(ex.Message);
        }
    }
}
