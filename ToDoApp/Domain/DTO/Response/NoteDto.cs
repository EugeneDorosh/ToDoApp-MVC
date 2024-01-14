using ToDoApp.Models;

namespace ToDoApp.DTO.Response
{
    public class NoteDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
    }
}
