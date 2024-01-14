using Microsoft.AspNetCore.Identity;

namespace ToDoApp.Models
{
    public class User : IdentityUser<Guid>
    {
        public ICollection<ToDoTask> Tasks { get; set; }
        public ICollection<Note> Notes { get; set; }
    }
}
