using Service.Interfaces;
using Service.Interface.IValidation;
using ToDoApp.DTO.Response;
using ToDoApp.Models;

namespace ToDoApp.Validation
{
    public class NoteValidation : INoteValidationToDoApp
    {
        private readonly IUserRepository _userRepository;

        public NoteValidation(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<bool> IsNoteValid(NoteDto noteDTO)
        {
            return true;
        }
    }
}
