namespace PruebaTecnica.Application.DTOs.Auth
{
    public class RefreshTokenRequest
    {
        public Guid UserId { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
