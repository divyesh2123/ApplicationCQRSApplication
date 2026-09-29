using ApplicationCQRSApplication.Features.Employees.Commands.CreateEmployee;
using ApplicationCQRSApplication.Features.Employees.Queries.GetEmployee;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCQRSApplication.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ISender _sender;

        public EmployeesController(ISender sender)
        {
            _sender = sender;
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


    }
}
