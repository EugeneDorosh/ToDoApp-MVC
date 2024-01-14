using ToDoApp.DTO;
using ToDoApp.Models;

namespace Service.Interfaces
{
    public interface INoteRepository : IBaseRepository
    {
        public Task<Note> GetNoteAsync(Guid id);
        public Task<ICollection<Note>> GetNotesAsync(Guid id);
        public Task<bool> CreateNoteAsync(Note note);
        public Task<bool> UpdateNoteAsync(Note note);
        public Task<bool> DeleteNoteAsync(Guid noteId);
        public Task<bool> NoteExistsAsync(Guid id);
    }
}
