using ApplicationCQRSApplication.DTOs;
using MediatR;

namespace ApplicationCQRSApplication.Features.Employees.Queries.GetEmployee
{

    public record GetEmployeeQuery(int Id)
        : IRequest<EmployeeDto?>;
}
