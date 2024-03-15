namespace Core.Models
{
    public class Token
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public string RefreshToken { get; set; }
        public string? UserGuid { get; set; }
        public bool IsAdmin { get; set; }
    }
}
