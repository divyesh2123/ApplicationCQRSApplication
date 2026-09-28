using ApplicationCQRSApplication.DTOs;
using ApplicationCQRSApplication.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCQRSApplication.Features.Employees.Queries.GetEmployee
{
    public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, EmployeeDto?>
    {
        private readonly AppDbContext _context;

        public GetEmployeeQueryHandler(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<EmployeeDto?> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
        {
            return await _context.Employees
           .AsNoTracking()
           .Where(e => e.Id == request.Id)
           .Select(e => new EmployeeDto
           {
               Id = e.Id,
               FirstName = e.FirstName,
               LastName = e.LastName,
               Email = e.Email,
               Department = e.Department,
               Salary = e.Salary,
               IsActive = e.IsActive
           })
           .FirstOrDefaultAsync(cancellationToken);
        }
   
        
    
    }
}
