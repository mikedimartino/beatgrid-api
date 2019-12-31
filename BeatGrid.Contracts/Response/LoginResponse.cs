namespace BeatGrid.Contracts.Response
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
        public string Username { get; set; }
    }
}
