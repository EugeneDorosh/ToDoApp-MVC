using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Runtime.InteropServices;
using ToDoApp.Data;
using ToDoApp.Models;
using ToDoApp.Models.Enums;

namespace ToDoApp.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ToDoContext _context;

        public TaskRepository(ToDoContext context)
        {
            _context = context;
        }
        public async Task<bool> ChangeStatusAsync(Guid id, Status status)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
                return false;

            task.Status = status;
            return await SaveAsync();
        }

        public async Task<bool> CreateTaskAsync(ToDoTask task)
        {
            _context.Add(task);
            return await SaveAsync();
        }

        public async Task<bool> DeleteTaskAsync(Guid id)
        {
            ToDoTask taskToDelete = _context.Tasks.FirstOrDefault(t => t.Id == id);
            _context.Tasks.Remove(taskToDelete);

            return await SaveAsync();
        }

        public async Task<ToDoTask> GetTaskAsync(Guid id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<ICollection<ToDoTask>> GetTasksAsync(Guid userId)
        {
            return await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> TaskExistsAsync(Guid id)
        {
            return await _context.Tasks.AnyAsync(t => t.Id == id);
        }

        public async Task<bool> UpdateTaskAsync(ToDoTask task)
        {
            _context.Update(task);
            return await SaveAsync();
        }
    }
}
