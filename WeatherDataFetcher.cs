using FoodDeliveryBackend.Data;
using FoodDeliveryBackend.Models;
using System.Net.Http;  // Odd... Why is this not used..?
using System.Xml.Linq;

namespace FoodDeliveryBackend
{
    /// <summary>
    /// Fetches the weather through a fixed xml page.
    /// </summary>
    public class WeatherDataFetcher
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly string _xmlUrl = "https://www.ilmateenistus.ee/ilma_andmed/xml/observations.php";

        /// <summary>
        /// Setup the application database context and http client.
        /// </summary>
        public WeatherDataFetcher(AppDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Fetches and stores the weather data. Data is stored in the database.
        /// </summary>
        public async Task FetchAndStoreWeatherDataAsync()
        {
            try
            {
                // Get the XML content as string
                string xmlContent = await _httpClient.GetStringAsync(_xmlUrl);

                // Parse it as an XML
                XDocument doc = XDocument.Parse(xmlContent);

                // Get timestamp
                var timestamp = doc.Element("observations")?.Attribute("timestamp")?.Value;
                
                // Parse it as a integer. If fails, the current timestamp is saved as unix time.
                int observationTime = int.TryParse(timestamp, out int time) 
                    ? time : (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

                // Use the WeatherTimestamp Model to save the time in the database later.
                var weatherTimestamp = new WeatherTimestamp
                {
                    ObservationTime = observationTime
                };

                // Add the data to the database and save it.
                _context.WeatherTimestamps.Add(weatherTimestamp);
                await _context.SaveChangesAsync();

                // Get all station observation data.
                var observations = doc.Descendants("station").Select(station => new WeatherObservation
                {
                    StationName = station.Element("name")?.Value ?? "Unknown",
                    WmoCode = station.Element("wmocode")?.Value ?? "N/A",
                    AirTemperature = float.TryParse(station.Element("airtemperature")?.Value, out float temp) ? temp : 0f,
                    WindSpeed = float.TryParse(station.Element("windspeed")?.Value, out float wind) ? wind : 0f,
                    WeatherPhenomenon = station.Element("phenomenon")?.Value ?? "Unknown",
                    WeatherTimestampId = weatherTimestamp.Id
                }).ToList();

                // Add and save the observations.
                await _context.WeatherObservations.AddRangeAsync(observations.FindAll(x => x.StationName == "Tallinn-Harku" || x.StationName == "Tartu-Tõravere" || x.StationName == "Pärnu"));
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching weather data: {ex.Message}");
            }
        }
    }

}
