using System.Net.Http.Json;
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

        private const string LoginUrl =
            "https://ez-staging-api.azurewebsites.net/api/External_Api/POS_Api/Invoke";

        public ExternalAuthService(
            HttpClient httpClient,
            ApplicationDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task<(bool Success, string Message, string UserCode, List<LocationDto> Locations)>
            LoginAsync(LoginRequest request)
        {
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
                // 1. Call external API
                var response = await _httpClient.PostAsJsonAsync(
                    LoginUrl,
                    externalRequest);

                // 2. Check HTTP status
                if (!response.IsSuccessStatusCode)
                {
                    return (
                        false,
                        "Invalid email or password.",
                        string.Empty,
                        new List<LocationDto>()
                    );
                }

                // 3. Read response JSON
                var responseContent =
                    await response.Content.ReadAsStringAsync();

                // Temporary debugging
                Console.WriteLine(
                    "========== EXTERNAL API RESPONSE ==========");

                Console.WriteLine(responseContent);

                Console.WriteLine(
                    "===========================================");

                // 4. Deserialize JSON
                var loginResponse =
                    JsonSerializer.Deserialize<ExternalLoginResponse>(
                        responseContent);

                if (loginResponse == null)
                {
                    return (
                        false,
                        "Invalid response from authentication server.",
                        string.Empty,
                        new List<LocationDto>()
                    );
                }

                // 5. Check Response_Body
                if (loginResponse.Response_Body == null ||
                    loginResponse.Response_Body.Count == 0)
                {
                    return (
                        false,
                        "No user information returned from authentication server.",
                        string.Empty,
                        new List<LocationDto>()
                    );
                }

                // 6. Get first user
                var userData =
                    loginResponse.Response_Body[0];

                // 7. Get User_Locations
                var locations =
                    userData.User_Locations;

                // 8. Save locations to SQL Server
                foreach (var location in locations)
                {
                    var existingLocation =
                        await _context.LocationDetails
                            .FirstOrDefaultAsync(x =>
                                x.Location_Code ==
                                location.Location_Code);

                    if (existingLocation == null)
                    {
                        var locationDetail = new LocationDetail
                        {
                            Location_Code =
                                location.Location_Code,

                            Location_Name =
                                location.Location_Name
                        };

                        _context.LocationDetails.Add(
                            locationDetail);
                    }
                    else
                    {
                        existingLocation.Location_Name =
                            location.Location_Name;
                    }
                }

                // 9. Save database changes
                await _context.SaveChangesAsync();

                // 10. Return locations
                return (
                    true,
                    "Login successful.",
                    userData.User_Code,
                    locations
                );
            }
            catch (HttpRequestException)
            {
                return (
                    false,
                    "Unable to connect to the authentication server.",
                    string.Empty,
                    new List<LocationDto>()
                );
            }
            catch (JsonException)
            {
                return (
                    false,
                    "Invalid response received from authentication server.",
                    string.Empty,
                    new List<LocationDto>()
                );
            }
            catch (Exception)
            {
                return (
                    false,
                    "An unexpected error occurred.",
                    string.Empty,
                    new List<LocationDto>()
                );
            }
        }
    }
}