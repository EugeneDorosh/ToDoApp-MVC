using ToDoApp.DTO.Response;
using ToDoApp.Models.Enums;
using ToDoApp.Validators;

namespace Tests
{
    public class ToDoAppTests_Services
    {
        [Fact]
        public async Task TaskValidation_NotValidData_ShouldFail()
        {
            //Arrange
            DateTime incorrectDateTime = Convert.ToDateTime("01/01/2020");
            var validator = new TaskValidation();
            var taskDto = new ToDoTaskDto() { Id = Guid.Empty, Deadline = incorrectDateTime.AddDays(1), Description = string.Empty, 
                Priority = Priority.Normal, Status = Status.NotCompleted };
            
            //Act
            var result = await validator.IsTaskValid(taskDto);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async Task TaskValidation_ValidData_ShouldPass()
        {
            //Arrange
            DateTime incorrectDateTime = Convert.ToDateTime("01/01/2020");
            var validator = new TaskValidation();
            var taskDto = new ToDoTaskDto()
            {
                Id = Guid.Empty,
                Deadline = incorrectDateTime.AddDays(1),
                Description = string.Empty,
                Priority = Priority.Normal,
                Status = Status.NotCompleted
            };

            //Act
            var result = await validator.IsTaskValid(taskDto);

            //Assert
            Assert.True(result);
        }
    }
}