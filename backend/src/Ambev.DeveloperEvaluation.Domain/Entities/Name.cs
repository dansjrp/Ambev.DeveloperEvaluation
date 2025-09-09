namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Value Object para o nome do usuário.
/// </summary>
public class Name
{
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;

    public Name() {}
    public Name(string firstname, string lastname)
    {
        Firstname = firstname;
        Lastname = lastname;
    }
}
