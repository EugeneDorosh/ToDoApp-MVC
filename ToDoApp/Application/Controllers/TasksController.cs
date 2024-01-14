using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Interface.IValidation;
using ToDoApp.DTO.Response;
using ToDoApp.Models;
using ToDoApp.Models.Enums;

namespace ToDoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITaskValidationToDoApp _taskValidation;
        private readonly IMapper _mapper;

        public TasksController(ITaskRepository taskRepository,
                               IUserRepository userRepository,
                               ITaskValidationToDoApp taskValidation,
                               IMapper mapper)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _taskValidation = taskValidation;
            _mapper = mapper;
        }

        [HttpPost("tasks")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ToDoTaskDto>))]
        [ProducesResponseType(400)]
        [Authorize]
        public async Task<IActionResult> GetTasksAsync(Guid userId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                bool doesUserExist = await _userRepository.UserExistsAsync(userId);
                if (!doesUserExist)
                    return NotFound();

                var tasks = await _taskRepository.GetTasksAsync(userId);
                IEnumerable<ToDoTaskDto> tasksDTO = _mapper.Map<IEnumerable<ToDoTaskDto>>(tasks);

                return Ok(tasksDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("tasks/{taskId}")]
        [ProducesResponseType(200, Type = typeof(ToDoTaskDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetTaskAsync(Guid taskId, [FromBody] Guid userId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                bool doesUserExist = await _userRepository.UserExistsAsync(userId);
                if (!doesUserExist)
                    return NotFound();

                bool doesTaskExist = await _taskRepository.TaskExistsAsync(taskId);
                if (!doesTaskExist)
                    return NotFound();

                ToDoTask taskDTO = await _taskRepository.GetTaskAsync(taskId);

                if (taskDTO.UserId != userId)
                    return StatusCode(403);

                ToDoTaskDto task = _mapper.Map<ToDoTaskDto>(taskDTO);

                return Ok(task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("tasks/new")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateTaskAsync([FromBody] ToDoTaskDto taskDTO,
                                        [FromQuery] Guid userId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                bool doesUserExist = await _userRepository.UserExistsAsync(userId);
                if (!doesUserExist)
                    return BadRequest();

                bool isTaskValid = await _taskValidation.IsTaskValid(taskDTO);
                if (!isTaskValid)
                    return BadRequest("Invalid task object.");

                ToDoTask task = _mapper.Map<ToDoTask>(taskDTO);
                task.UserId = userId;

                if (!await _taskRepository.CreateTaskAsync(task))
                    return BadRequest("Couldnt create task.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("tasks/{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateTaskAsync([FromBody] ToDoTaskDto taskDTO, Guid userId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                bool isTaskValid = await _taskValidation.IsTaskValid(taskDTO);
                if (!isTaskValid)
                    return BadRequest("Invalid task object.");

                bool doesUserExist = await _userRepository.UserExistsAsync(userId);
                if (!doesUserExist)
                    return BadRequest();

                bool doesTaskExist = await _taskRepository.TaskExistsAsync(taskDTO.Id);
                if (!doesTaskExist)
                    return NotFound();

                ToDoTask task = _mapper.Map<ToDoTask>(taskDTO);
                task.UserId = userId;

                if (!await _taskRepository.UpdateTaskAsync(task))
                    return BadRequest("Couldnt create task.");

                return NoContent();
            }
            catch (Exception ex) 
            { 
                return StatusCode(500, ex.Message); 
            }
        }

        [HttpDelete("tasks/{taskId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteTaskAsync(Guid taskId, [FromBody] Guid userId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                bool doesUserExist = await _userRepository.UserExistsAsync(userId);
                if (!doesUserExist)
                    return BadRequest();

                bool doesTaskExist = await _taskRepository.TaskExistsAsync(taskId);
                if (!doesTaskExist)
                    return NotFound();

                var taskToDelete = await _taskRepository.GetTaskAsync(taskId);

                if (taskToDelete.UserId != userId)
                    return StatusCode(403);

                if (!await _taskRepository.DeleteTaskAsync(taskId))
                    return BadRequest("Couldnt delete task.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
