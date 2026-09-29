using ApplicationCQRSApplication.Models;
using MediatR;

namespace ApplicationCQRSApplication.Features.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeHandler :
        IRequestHandler<CreateEmployeeCommand, bool>
    {
        private readonly AppDbContext _context;

        public CreateEmployeeHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Department = request.Department,
                Salary = request.Salary
            };

            _context.Employees.Add(employee);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
