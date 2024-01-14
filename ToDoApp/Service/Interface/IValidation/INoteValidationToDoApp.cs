using ToDoApp.DTO.Response;
using ToDoApp.Models;

namespace Service.Interface.IValidation
{
    public interface INoteValidationToDoApp
    {
        public Task<bool> IsNoteValid(NoteDto noteDTO);
    }
}
