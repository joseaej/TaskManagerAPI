using System.ComponentModel.DataAnnotations;

namespace TasksManagerAPI.Models.Entity
{
    public class Account
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }

    }
}
