namespace NetHire.DTO
{
    public class LoginResponse
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }

    }
} 