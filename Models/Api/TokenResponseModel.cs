namespace TasksManagerAPI.Models.Api
{
    public class TokenResponseModel
    {
        public string? UserName { get; set; }
        public string? AccessToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
