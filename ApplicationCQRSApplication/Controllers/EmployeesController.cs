using ApplicationCQRSApplication.Data;
using ApplicationCQRSApplication.DTOs;
using ApplicationCQRSApplication.Features.Employees.Commands.CreateEmployee;
using ApplicationCQRSApplication.Features.Employees.Queries.GetEmployee;
using ApplicationCQRSApplication.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCQRSApplication.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ISender _sender;

        private readonly IGenericRepository<Employee> _repository;

        public EmployeesController(ISender sender, IGenericRepository<Employee> repository)
        {
            _sender = sender;
            _repository = repository;

        }

        [HttpGet]
        public async Task<IActionResult> GetById(
      int id,
      CancellationToken cancellationToken)
        {
            var result = await _sender.Send(
                new GetEmployeeQuery(id),
                cancellationToken);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

      public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(
        CreateEmployeeCommand command)
        {
            var employeeId = await _sender.Send(command);

            return RedirectToAction("Index");
        }


        [HttpGet]

        public async Task<IActionResult> IndexInfo(
       GridRequest request)
        {
            var result = await _repository.GetPagedAsync(request);

            return Ok(result);
        }


    }
}
