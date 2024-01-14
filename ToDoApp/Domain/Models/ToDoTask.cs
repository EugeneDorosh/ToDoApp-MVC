using ToDoApp.Models.Enums;

namespace ToDoApp.Models
{
    public class ToDoTask
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public DateTime? DateTime { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Status Status { get; set; }
    }
}