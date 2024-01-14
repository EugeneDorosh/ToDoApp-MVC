using ToDoApp.Models;
using ToDoApp.Models.Enums;

namespace Service.Interfaces
{
    public interface ITaskRepository : IBaseRepository
    {
        public Task<ToDoTask> GetTaskAsync(Guid id);
        public Task<ICollection<ToDoTask>> GetTasksAsync(Guid userId);
        public Task<bool> CreateTaskAsync(ToDoTask task);
        public Task<bool> UpdateTaskAsync(ToDoTask task);
        public Task<bool> DeleteTaskAsync(Guid id);
        public Task<bool> ChangeStatusAsync(Guid id, Status status);
        public Task<bool> TaskExistsAsync(Guid id);
    }
}
