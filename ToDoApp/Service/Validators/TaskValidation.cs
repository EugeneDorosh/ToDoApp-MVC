using Service.Interface.IValidation;
using ToDoApp.DTO.Response;

namespace ToDoApp.Validators
{
    public class TaskValidation : ITaskValidationToDoApp
    {
        public async Task<bool> IsTaskValid(ToDoTaskDto taskDTO)
        {
            if (taskDTO == null) 
                return false;

            if (taskDTO.Deadline <= DateTime.Now)
                return false;

            if ((int)taskDTO.Priority is < 0 or > 2)
                return false;

            if ((int)taskDTO.Status is < 0 or > 1)
                return false;

            return true;
        }
    }
}
