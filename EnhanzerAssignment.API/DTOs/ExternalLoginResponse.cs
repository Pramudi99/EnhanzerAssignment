using System.Text.Json.Serialization;

namespace EnhanzerAssignment.API.DTOs
{
    public class ExternalLoginResponse
    {
        [JsonPropertyName("Status_Code")]
        public int Status_Code { get; set; }

        [JsonPropertyName("Sync_Time")]
        public string Sync_Time { get; set; } = string.Empty;

        [JsonPropertyName("Message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("Response_Body")]
        public List<LoginResponseBody> Response_Body { get; set; } = new();
    }

    public class LoginResponseBody
    {
        [JsonPropertyName("User_Code")]
        public string User_Code { get; set; } = string.Empty;

        [JsonPropertyName("User_Display_Name")]
        public string User_Display_Name { get; set; } = string.Empty;

        [JsonPropertyName("Email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("User_Employee_Code")]
        public string User_Employee_Code { get; set; } = string.Empty;

        [JsonPropertyName("Company_Code")]
        public string Company_Code { get; set; } = string.Empty;

        [JsonPropertyName("User_Locations")]
        public List<LocationDto> User_Locations { get; set; } = new();
    }
}