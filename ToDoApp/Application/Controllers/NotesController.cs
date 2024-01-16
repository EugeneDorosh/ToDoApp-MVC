using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Interface.IValidation;
using ToDoApp.DTO.Response;
using ToDoApp.Models;
using Application.Controllers;
using Domain.DTO.Request;

namespace ToDoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : Controller
    {
        private readonly INoteRepository _noteRepository;
        private readonly IUserRepository _userRepository;
        private readonly INoteValidationToDoApp _noteValidation;
        private readonly IMapper _mapper;

        public NotesController(INoteRepository noteRepository,
                               IUserRepository userRepository,
                               INoteValidationToDoApp noteValidation,
                               IMapper mapper)
        {
            _noteRepository = noteRepository;
            _userRepository = userRepository;
            _noteValidation = noteValidation;
            _mapper = mapper;
        }

        [HttpPost("mvc")]
        [ProducesResponseType(200, Type = typeof(NoteDto))]
        [ProducesResponseType(400)]
        [AllowAnonymous]
        public async Task<IActionResult> DemonstrateMVC(Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                bool doesUserExist = await _userRepository.UserExistsAsync(id);
                if (!doesUserExist)
                    return NotFound("User not found.");

                var notes = await _noteRepository.GetNotesAsync(id);
                var notesDTO = _mapper.Map<IEnumerable<NoteDto>>(notes);

                return View(notesDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("notes")]
        [ProducesResponseType(200, Type = typeof(NoteDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetNotes(Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                bool doesUserExist = await _userRepository.UserExistsAsync(id);
                if (!doesUserExist)
                    return NotFound("User not found.");

                var notes = await _noteRepository.GetNotesAsync(id);
                var notesDTO = _mapper.Map<IEnumerable<NoteDto>>(notes);

                return Ok(notesDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("notes/{noteId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<NoteDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetNoteAsync(Guid noteId, [FromBody] Guid userId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Note note = await _noteRepository.GetNoteAsync(noteId);

                if (note == null)
                    return NotFound();

                if (userId != note.UserId)
                    return StatusCode(403);

                NoteDto noteDTO = _mapper.Map<NoteDto>(note);

                return Ok(noteDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("notes/new")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateNoteAsync([FromBody] NoteDtoRequest noteDTO, Guid userId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                bool doesUserExist = await _userRepository.UserExistsAsync(userId);
                if (!doesUserExist)
                    return NotFound("User not found.");

                bool isNoteValid = await _noteValidation.IsNoteValid(noteDTO);
                if (!isNoteValid)
                    return BadRequest("Incorrect note object.");

                Note note = _mapper.Map<Note>(noteDTO);
                note.UserId = userId;

                if (!await _noteRepository.CreateNoteAsync(note))
                    return BadRequest("Couldnt create note.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("notes/{noteId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteNoteAsync([FromBody] Guid userId, Guid noteId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                bool doesUserExist = await _userRepository.UserExistsAsync(userId);
                if (!doesUserExist)
                    return NotFound("User not found.");

                await _noteRepository.DeleteNoteAsync(noteId);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("notes/{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateNoteAsync([FromBody] NoteDto noteDTO, Guid userId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                bool doesUserExist = await _userRepository.UserExistsAsync(userId);
                if (!doesUserExist)
                    return NotFound("User not found.");

                bool isNoteValid = await _noteValidation.IsNoteValid(noteDTO);
                if (!isNoteValid)
                    return BadRequest("Incorrect note object.");

                Note note = _mapper.Map<Note>(noteDTO);
                note.UserId = userId;

                if (!await _noteRepository.UpdateNoteAsync(note))
                    return BadRequest();

                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
