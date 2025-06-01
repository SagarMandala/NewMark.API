using NewMark.API.Model;
using System.Text.Json;

namespace NewMark.API.Service
{
    public class BlobService : IBlobService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<BlobService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public BlobService(IConfiguration config, ILogger<BlobService> logger, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Fetches a JSON file from Azure Blob Storage using the Blob URL and SAS token from configuration.
        /// Performs HTTP GET, checks for errors, deserializes the response into a BlobData object, and logs any issues.
        /// </summary>
        /// <returns>BlobData object representing the JSON content from the blob file.</returns>
        /// <exception cref="HttpRequestException">Thrown if HTTP request fails (e.g., 404, 403, etc.).</exception>
        /// <exception cref="JsonException">Thrown if JSON deserialization fails.</exception>
        /// <exception cref="Exception">Thrown for other unhandled exceptions.</exception>
        public async Task<List<Property>> GetBlobDataAsync()
        {
            try
            {
                var blobUrl = _config["BlobStorage:BlobUrl"];
                var sasToken = _config["BlobStorage:SasToken"];
                var fullUrl = blobUrl + sasToken;

                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(fullUrl);

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<Property>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result == null)
                    throw new Exception("Deserialized object is null.");

                return result;
            }
            catch (HttpRequestException ex)
            {
                LogToFile($"HTTP error: {ex.Message}");
                _logger.LogError(ex, "HttpRequestException occurred while accessing blob.");
                throw;
            }
            catch (JsonException ex)
            {
                LogToFile($"JSON error: {ex.Message}");
                _logger.LogError(ex, "JsonException occurred while parsing blob data.");
                throw;
            }
            catch (Exception ex)
            {
                LogToFile($"General error: {ex.Message}");
                _logger.LogError(ex, "Unexpected error in BlobService.");
                throw;
            }
        }

        private void LogToFile(string message)
        {
            var path = _config["Logging:LogFilePath"] ?? "blob-error-log.txt";
            var logEntry = $"{DateTime.UtcNow:u} | {message}{Environment.NewLine}";
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.AppendAllText(path, logEntry);
        }
    }
}
