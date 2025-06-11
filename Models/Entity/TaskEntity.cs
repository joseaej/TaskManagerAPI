using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TasksManagerAPI.Models.Entity
{
    public class TaskEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string TaskName { get; set; }

        public Project? Project { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        public string? Description { get; set; }

        public List<Account> AccountsUsername { get; set; } = new List<Account>();
    }
}
