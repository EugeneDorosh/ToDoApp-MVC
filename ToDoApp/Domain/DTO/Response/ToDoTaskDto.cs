using ToDoApp.Models.Enums;
using ToDoApp.Models;

namespace ToDoApp.DTO.Response
{
    public class ToDoTaskDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public DateTime? Deadline { get; set; }
        public Status Status { get; set; }
    }
}
