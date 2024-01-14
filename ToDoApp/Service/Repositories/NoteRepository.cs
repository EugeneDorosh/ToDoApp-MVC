using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using ToDoApp.Data;
using ToDoApp.DTO;
using ToDoApp.Models;

namespace ToDoApp.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly ToDoContext _context;

        public NoteRepository(ToDoContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateNoteAsync(Note note)
        {
            _context.Add(note);
            return await SaveAsync();
        }

        public async Task<bool> DeleteNoteAsync(Guid noteId)
        {
            var note = _context.Notes.FirstOrDefaultAsync(x => x.Id == noteId);
            _context.Remove(note);
            return await SaveAsync();
        }

        public async Task<Note> GetNoteAsync(Guid id)
        {
            return await _context.Notes.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<ICollection<Note>> GetNotesAsync(Guid id)
        {
            return await _context.Notes.Where(n => n.UserId == id).ToListAsync();
        }

        public async Task<bool> NoteExistsAsync(Guid id)
        {
            return await _context.Notes.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateNoteAsync(Note note)
        {
            _context.Update(note);
            return await SaveAsync();
        }
    }
}
