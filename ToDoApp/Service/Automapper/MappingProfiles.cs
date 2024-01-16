using AutoMapper;
using Domain.DTO.Request;
using Microsoft.AspNetCore.Identity;
using ToDoApp.DTO.Response;
using ToDoApp.Models;

namespace ToDoApp.Automapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Note, NoteDto>().ReverseMap();
            CreateMap<Note, NoteDtoRequest>().ReverseMap();
            CreateMap<ToDoTask, ToDoTaskDto>().ReverseMap();
            CreateMap<UserRegistrationDto, User>();
        }
    }
}
