using ToDoApp.Models;
using ToDoApp.DTO.Response;
using System.ComponentModel.DataAnnotations;
using Service.Interfaces;
using Service.Interface.IValidation;

namespace ToDoApp.Validation
{
    public class UserValidation : IUserValidationToDoApp
    {
        private readonly IUserRepository _userRepository;

        public UserValidation(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<bool> IsUserValidAsync(UserDto userDto)
        {
            if (userDto == null)
                return false;

            if (userDto.Username == null)
                return false;

            if (!await _userRepository.IsUsernameUnique(userDto.Username))
                return false;

            return true;
        }

        public async Task<bool> IsUserValidAsync(UserRegistrationDto userDto)
        {
            if (userDto == null)
                return false;

            if (userDto.Username == null)
                return false;

            if (await _userRepository.IsUsernameUnique(userDto.Username))
                return false;

            var email = new EmailAddressAttribute();
            if (!email.IsValid(userDto.Email))
                return false;

            return true;
        }
    }
}
