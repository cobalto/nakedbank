namespace NakedBank.Shared.Models.Responses
{
    public class AuthResponse : BaseResponse
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string Token { get; set; }

        public AuthResponse() : base() { }
    }
}
