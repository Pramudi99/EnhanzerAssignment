using System.Text;
using System.Text.Json;
using EnhanzerAssignment.API.Data;
using EnhanzerAssignment.API.DTOs;
using EnhanzerAssignment.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EnhanzerAssignment.API.Services
{
    public class ExternalAuthService : IExternalAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ExternalAuthService> _logger;

        private const string LoginUrl =
            "https://ez-staging-api.azurewebsites.net/api/External_Api/POS_Api/Invoke";

        public ExternalAuthService(
            HttpClient httpClient,
            ApplicationDbContext context,
            ILogger<ExternalAuthService> logger)
        {
            _httpClient = httpClient;
            _context = context;
            _logger = logger;
        }

        public async Task<(
            bool Success,
            string Message,
            string UserCode,
            List<LocationDto> Locations
        )> LoginAsync(LoginRequest request)
        {
            try
            {
                // Create request for the external authentication API
                var externalRequest = new ExternalLoginRequest
                {
                    API_Action = "GetLoginData",
                    Device_Id = "D001",
                    Sync_Time = string.Empty,
                    Company_Code = request.Email,
                    API_Body = new ExternalLoginBody
                    {
                        Username = request.Email,
                        Pw = request.Password
                    }
                };

                // Serialize request to JSON
                var json = JsonSerializer.Serialize(externalRequest);

                using var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                // Call external authentication API
                var response = await _httpClient.PostAsync(
                    LoginUrl,
                    content);

                var responseContent =
                    await response.Content.ReadAsStringAsync();

                // Check HTTP response
                if (!response.IsSuccessStatusCode)
                {
                    return (
                        false,
                        "Invalid email or password.",
                        string.Empty,
                        new List<LocationDto>()
                    );
                }

                // Deserialize external API response
                var loginResponse =
                    JsonSerializer.Deserialize<ExternalLoginResponse>(
                        responseContent);

                if (loginResponse == null)
                {
                    return (
                        false,
                        "Unable to process the authentication response.",
                        string.Empty,
                        new List<LocationDto>()
                    );
                }

                // Check whether a valid user was returned
                if (loginResponse.Response_Body == null ||
                    loginResponse.Response_Body.Count == 0)
                {
                    return (
                        false,
                        "Invalid email or password.",
                        string.Empty,
                        new List<LocationDto>()
                    );
                }

                var userData = loginResponse.Response_Body[0];

                // Get locations assigned to the authenticated user
                var locations = userData.User_Locations ?? new List<LocationDto>();

                // Save or update locations in the database
                foreach (var location in locations)
                {
                    var existingLocation =
                        await _context.LocationDetails
                            .FirstOrDefaultAsync(x =>
                                x.Location_Code == location.Location_Code);

                    if (existingLocation == null)
                    {
                        _context.LocationDetails.Add(
                            new LocationDetail
                            {
                                Location_Code = location.Location_Code,
                                Location_Name = location.Location_Name
                            });
                    }
                    else
                    {
                        existingLocation.Location_Name =
                            location.Location_Name;
                    }
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "External authentication successful for user {UserCode}. {LocationCount} locations processed.",
                    userData.User_Code,
                    locations.Count);

                return (
                    true,
                    "Login successful.",
                    userData.User_Code,
                    locations
                );
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(
                    ex,
                    "Error connecting to the external authentication API.");

                return (
                    false,
                    "Unable to connect to the authentication service. Please try again later.",
                    string.Empty,
                    new List<LocationDto>()
                );
            }
            catch (JsonException ex)
            {
                _logger.LogError(
                    ex,
                    "Invalid JSON response received from the external authentication API.");

                return (
                    false,
                    "Invalid response received from the authentication service.",
                    string.Empty,
                    new List<LocationDto>()
                );
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Error saving location details to the database.");

                return (
                    false,
                    "Unable to save location information. Please try again later.",
                    string.Empty,
                    new List<LocationDto>()
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unexpected error occurred during external authentication.");

                return (
                    false,
                    "An unexpected error occurred. Please try again later.",
                    string.Empty,
                    new List<LocationDto>()
                );
            }
        }
    }
}