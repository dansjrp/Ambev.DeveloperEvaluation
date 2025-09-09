namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Value Object para geolocalização.
/// </summary>
public class Geolocation
{
    public string Lat { get; set; } = string.Empty;
    public string Long { get; set; } = string.Empty;

    public Geolocation() {}
    public Geolocation(string lat, string lng)
    {
        Lat = lat;
        Long = lng;
    }
}
