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
            // ---------------------------------------------------------
            // 1. Create request for external API
            // ---------------------------------------------------------

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

            try
            {
                // ---------------------------------------------------------
                // 2. Serialize request
                // ---------------------------------------------------------

                var json = JsonSerializer.Serialize(
                    externalRequest);

                using var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");


                // ---------------------------------------------------------
                // 3. Diagnostic logging
                // ---------------------------------------------------------
                // IMPORTANT:
                // We do NOT log the password.
                // ---------------------------------------------------------

                _logger.LogWarning(
                    "========================================");

                _logger.LogWarning(
                    "START EXTERNAL LOGIN");

                _logger.LogWarning(
                    "External API URL: {Url}",
                    LoginUrl);

                _logger.LogWarning(
                    "API Action: {Action}",
                    externalRequest.API_Action);

                _logger.LogWarning(
                    "Device ID: {Device}",
                    externalRequest.Device_Id);

                _logger.LogWarning(
                    "Company Code present: {Present}",
                    !string.IsNullOrWhiteSpace(
                        externalRequest.Company_Code));

                _logger.LogWarning(
                    "Username present: {Present}",
                    !string.IsNullOrWhiteSpace(
                        externalRequest.API_Body?.Username));

                _logger.LogWarning(
                    "Password present: {Present}",
                    !string.IsNullOrWhiteSpace(
                        externalRequest.API_Body?.Pw));


                // ---------------------------------------------------------
                // 4. Call external API
                // ---------------------------------------------------------

                var response = await _httpClient.PostAsync(
                    LoginUrl,
                    content);


                // ---------------------------------------------------------
                // 5. Read external API response
                // ---------------------------------------------------------

                var responseContent =
                    await response.Content.ReadAsStringAsync();


                // ---------------------------------------------------------
                // 6. Log HTTP status
                // ---------------------------------------------------------

                _logger.LogWarning(
                    "EXTERNAL API HTTP STATUS: {StatusCode}",
                    response.StatusCode);


                // ---------------------------------------------------------
                // 7. Log actual external API response
                // ---------------------------------------------------------

                _logger.LogWarning(
                    "EXTERNAL API RESPONSE: {Response}",
                    responseContent);


                // ---------------------------------------------------------
                // 8. Check HTTP status
                // ---------------------------------------------------------

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "External API returned unsuccessful HTTP status.");

                    _logger.LogWarning(
                        "END EXTERNAL LOGIN");

                    return (
                        false,
                        "Invalid email or password.",
                        string.Empty,
                        new List<LocationDto>()
                    );
                }


                // ---------------------------------------------------------
                // 9. Deserialize external API response
                // ---------------------------------------------------------

                var loginResponse =
                    JsonSerializer.Deserialize<ExternalLoginResponse>(
                        responseContent,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });


                // ---------------------------------------------------------
                // 10. Check if deserialization succeeded
                // ---------------------------------------------------------

                if (loginResponse == null)
                {
                    _logger.LogWarning(
                        "External API response could not be deserialized.");

                    _logger.LogWarning(
                        "END EXTERNAL LOGIN");

                    return (
                        false,
                        "Invalid response from authentication server.",
                        string.Empty,
                        new List<LocationDto>()
                    );
                }


                // ---------------------------------------------------------
                // 11. Diagnostic information about Response_Body
                // ---------------------------------------------------------

                _logger.LogWarning(
                    "Response_Body is null: {IsNull}",
                    loginResponse.Response_Body == null);

                _logger.LogWarning(
                    "Response_Body count: {Count}",
                    loginResponse.Response_Body?.Count ?? 0);


                // ---------------------------------------------------------
                // 12. Check Response_Body
                // ---------------------------------------------------------

                if (loginResponse.Response_Body == null ||
                    loginResponse.Response_Body.Count == 0)
                {
                    _logger.LogWarning(
                        "No user information returned by external API.");

                    _logger.LogWarning(
                        "END EXTERNAL LOGIN");

                    return (
                        false,
                        "No user information returned from authentication server.",
                        string.Empty,
                        new List<LocationDto>()
                    );
                }


                // ---------------------------------------------------------
                // 13. Get first user
                // ---------------------------------------------------------

                var userData =
                    loginResponse.Response_Body[0];


                _logger.LogWarning(
                    "User information received successfully.");

                _logger.LogWarning(
                    "User Code present: {Present}",
                    !string.IsNullOrWhiteSpace(
                        userData.User_Code));


                // ---------------------------------------------------------
                // 14. Get user locations
                // ---------------------------------------------------------

                var locations =
                    userData.User_Locations ?? new List<LocationDto>();


                _logger.LogWarning(
                    "User location count: {Count}",
                    locations.Count);


                // ---------------------------------------------------------
                // 15. Save locations to SQL Server
                // ---------------------------------------------------------

                foreach (var location in locations)
                {
                    _logger.LogWarning(
                        "Processing location. Code={Code}, Name={Name}",
                        location.Location_Code,
                        location.Location_Name);


                    // Check whether location already exists

                    var existingLocation =
                        await _context.LocationDetails
                            .FirstOrDefaultAsync(x =>
                                x.Location_Code ==
                                location.Location_Code);


                    // -----------------------------------------------------
                    // 16. Add new location
                    // -----------------------------------------------------

                    if (existingLocation == null)
                    {
                        var locationDetail =
                            new LocationDetail
                            {
                                Location_Code =
                                    location.Location_Code,

                                Location_Name =
                                    location.Location_Name
                            };


                        _context.LocationDetails.Add(
                            locationDetail);


                        _logger.LogWarning(
                            "New location added: {LocationCode}",
                            location.Location_Code);
                    }


                    // -----------------------------------------------------
                    // 17. Update existing location
                    // -----------------------------------------------------

                    else
                    {
                        existingLocation.Location_Name =
                            location.Location_Name;


                        _logger.LogWarning(
                            "Existing location updated: {LocationCode}",
                            location.Location_Code);
                    }
                }


                // ---------------------------------------------------------
                // 18. Save database changes
                // ---------------------------------------------------------

                await _context.SaveChangesAsync();


                _logger.LogWarning(
                    "Locations saved successfully to database.");


                // ---------------------------------------------------------
                // 19. Login successful
                // ---------------------------------------------------------

                _logger.LogWarning(
                    "LOGIN SUCCESSFUL");

                _logger.LogWarning(
                    "END EXTERNAL LOGIN");

                _logger.LogWarning(
                    "========================================");


                return (
                    true,
                    "Login successful.",
                    userData.User_Code,
                    locations
                );
            }


            // =============================================================
            // 20. HTTP REQUEST EXCEPTION
            // =============================================================

            catch (HttpRequestException ex)
            {
                _logger.LogError(
                    ex,
                    "HTTP error while calling external authentication API.");


                return (
                    false,
                    "Unable to connect to the authentication server.",
                    string.Empty,
                    new List<LocationDto>()
                );
            }


            // =============================================================
            // 21. JSON EXCEPTION
            // =============================================================

            catch (JsonException ex)
            {
                _logger.LogError(
                    ex,
                    "JSON deserialization error from external authentication API.");


                return (
                    false,
                    "Invalid response received from authentication server.",
                    string.Empty,
                    new List<LocationDto>()
                );
            }


            // =============================================================
            // 22. DATABASE EXCEPTION
            // =============================================================

            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Database error while saving locations.");


                return (
                    false,
                    "Unable to save location information.",
                    string.Empty,
                    new List<LocationDto>()
                );
            }


            // =============================================================
            // 23. GENERAL EXCEPTION
            // =============================================================

            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unexpected error during login and location saving.");


                return (
                    false,
                    $"Unexpected error: {ex.Message}",
                    string.Empty,
                    new List<LocationDto>()
                );
            }
        }
    }
}