using ToDoApp.DTO.Response;
using ToDoApp.Models;

namespace Service.Interface.IValidation
{
    public interface IUserValidationToDoApp
    {
        public Task<bool> IsUserValidAsync(UserDto userDTO);
        public Task<bool> IsUserValidAsync(UserRegistrationDto userDto);
    }
}
