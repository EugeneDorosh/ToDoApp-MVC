using Domain.DTO.Request;
using ToDoApp.DTO.Response;

namespace Service.Interface.IValidation
{
    public interface INoteValidationToDoApp
    {
        public Task<bool> IsNoteValid(NoteDto noteDTO);
        public Task<bool> IsNoteValid(NoteDtoRequest noteDTO);
    }
}
