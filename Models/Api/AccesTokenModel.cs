namespace TasksManagerAPI.Models.Api
{
    public class AccesTokenModel
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }


        public AccesTokenModel(string? userName, string? password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
