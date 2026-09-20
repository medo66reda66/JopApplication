namespace JopApplication.Application.Dtos
{
    public enum LoginStatus
    {
        Success,
        UserNotFound,
        InvalidPassword,
        EmailNotConfirmed,
        LockedOut
    }
    public class LoginggResult
    {
        public LoginStatus Status { get; set; }
        public string Token { get; set; }
        public string Validtoken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string RefreshTokenTime { get; set; }
    }
}