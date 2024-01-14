using System.ComponentModel.DataAnnotations;
using ToDoApp.Models;

namespace ToDoApp.DTO.Response
{
    public class UserRegistrationDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
