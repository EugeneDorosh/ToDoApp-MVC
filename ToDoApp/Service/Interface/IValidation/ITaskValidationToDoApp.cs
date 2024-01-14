using ToDoApp.DTO.Response;
using ToDoApp.Models;

namespace Service.Interface.IValidation
{
    public interface ITaskValidationToDoApp
    {
        public Task<bool> IsTaskValid(ToDoTaskDto taskDTO);
    }
}
