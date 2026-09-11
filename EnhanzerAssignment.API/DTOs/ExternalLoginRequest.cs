using System.Text.Json.Serialization;

namespace EnhanzerAssignment.API.DTOs
{
    public class ExternalLoginRequest
    {
        [JsonPropertyName("API_Action")]
        public string API_Action { get; set; } = "GetLoginData";

        [JsonPropertyName("Device_Id")]
        public string Device_Id { get; set; } = "D001";

        [JsonPropertyName("Sync_Time")]
        public string Sync_Time { get; set; } = string.Empty;

        [JsonPropertyName("Company_Code")]
        public string Company_Code { get; set; } = string.Empty;

        [JsonPropertyName("API_Body")]
        public ExternalLoginBody API_Body { get; set; } = new();
    }

    public class ExternalLoginBody
    {
        [JsonPropertyName("Username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("Pw")]
        public string Pw { get; set; } = string.Empty;
    }
}
