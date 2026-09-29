using MediatR;

namespace ApplicationCQRSApplication.Features.Employees.Commands.CreateEmployee
{
    public record CreateEmployeeCommand(
     string FirstName,
     string LastName,   
     string Email,
     string Department,
     decimal Salary
 ) : IRequest<bool>;
}
