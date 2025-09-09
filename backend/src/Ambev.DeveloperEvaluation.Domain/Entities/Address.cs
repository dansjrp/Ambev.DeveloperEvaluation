namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Value Object para endereço do usuário.
/// </summary>
public class Address
{
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public int Number { get; set; }
    public string Zipcode { get; set; } = string.Empty;
    public Geolocation Geolocation { get; set; } = new Geolocation();

    public Address() {}
    public Address(string city, string street, int number, string zipcode, Geolocation geolocation)
    {
        City = city;
        Street = street;
        Number = number;
        Zipcode = zipcode;
        Geolocation = geolocation;
    }
}
