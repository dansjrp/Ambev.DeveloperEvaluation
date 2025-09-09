using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser;

public class GetUsersResult
{
    public IEnumerable<GetUserResult> Users { get; set; } = new List<GetUserResult>();
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
}
