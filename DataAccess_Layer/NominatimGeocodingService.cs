using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
public class GeocodeResult
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
public class NominatimResult
{
    public string lat { get; set; }
    public string lon { get; set; }
}

public class NominatimGeocodingService
{
    private static readonly HttpClient client = new HttpClient();

    public NominatimGeocodingService()
    {
        // Nominatim requires a valid User-Agent. Replace with your app info.
        client.DefaultRequestHeaders.Add("User-Agent", "MyApp/1.0 (contact: myemail@example.com)");
    }

    /// <summary>
    /// Receives an address and returns a GeocodeResult containing the latitude and longitude.
    /// </summary>
    /// <param name="address">The address to geocode.</param>
    /// <returns>A GeocodeResult with Latitude and Longitude.</returns>
    public async Task<GeocodeResult> GetCoordinatesAsync(string address)
    {
        // Build the request URL, limiting to one result.
        string url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(address)}&format=json&limit=1";

        try
        {
            string jsonResponse = await client.GetStringAsync(url);

            // Deserialize the response into a list of NominatimResult objects.
            List<NominatimResult> results = JsonSerializer.Deserialize<List<NominatimResult>>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (results != null && results.Count > 0)
            {
                // Use the first result.
                var firstResult = results[0];
                return new GeocodeResult
                {
                    Latitude = double.Parse(firstResult.lat, System.Globalization.CultureInfo.InvariantCulture),
                    Longitude = double.Parse(firstResult.lon, System.Globalization.CultureInfo.InvariantCulture)
                };
            }
            else
            {
                throw new Exception("No geocoding results found.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error during geocoding: {ex.Message}");
        }
    }

    public async Task ExampleUsageAsync()
    {
        var geocodingService = new NominatimGeocodingService();
        string address = "100 Rue Didouche Mourad, Alger Centre, Algeria";
        try
        {
            GeocodeResult result = await geocodingService.GetCoordinatesAsync(address);
            Console.WriteLine($"Latitude: {result.Latitude}, Longitude: {result.Longitude}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

}
