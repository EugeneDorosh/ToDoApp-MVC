using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Interface.IValidation;
using ToDoApp.DTO.Response;
using ToDoApp.Models;
using Service.Interface.ITokenHandler;

namespace ToDoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserValidationToDoApp _userValidation;
        private readonly ITokenHandler _tokenHandler;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository,
                               IUserValidationToDoApp userValidation,
                               ITokenHandler tokenHandler,
                               IMapper mapper)
        {
            _userRepository = userRepository;
            _userValidation = userValidation;
            _tokenHandler = tokenHandler;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        public async Task<IActionResult> GetUserAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var token = HttpContext.Request.Cookies["jwt"];

                Guid id = default;
                if (!_tokenHandler.TryGetIdFromJwtToken(token, ref id))
                    return BadRequest("Invalid token.");

                User user = await _userRepository.GetUserAsync(id);

                if (user == null)
                    return NotFound();

                UserDto userDTO = _mapper.Map<UserDto>(user);

                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("users")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteUserAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var token = HttpContext.Request.Cookies["jwt"];

                Guid id = default;
                if (!_tokenHandler.TryGetIdFromJwtToken(token, ref id))
                    return BadRequest("Invalid token.");

                if (!await _userRepository.DeleteUserAsync(id))
                    return BadRequest("Something happen during remove user.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("users")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UserDto userToUpdate)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var token = HttpContext.Request.Cookies["jwt"];

                Guid id = default;
                if (!_tokenHandler.TryGetIdFromJwtToken(token, ref id))
                    return BadRequest("Invalid token.");

                if (!await _userValidation.IsUserValidAsync(userToUpdate))
                    return BadRequest("Incorrect user object.");

                User user = _mapper.Map<User>(userToUpdate);

                if (!await _userRepository.UpdateUserAsync(user))
                    return BadRequest("Something happen during updating user.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
