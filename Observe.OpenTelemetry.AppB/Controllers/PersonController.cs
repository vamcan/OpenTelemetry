using Microsoft.AspNetCore.Mvc;
using Observe.OpenTelemetry.AppB.Model;

namespace Observe.OpenTelemetry.AppB.Controllers
{
    public class PersonController : Controller
    {
        private readonly PersonRepo _personRepo;

        public PersonController(PersonRepo personRepo)
        {
            _personRepo = personRepo;
        }

        [HttpGet("GetAllA")]
        public async Task<IActionResult> Get()
        {
           var result = await _personRepo.Get();
           return Ok(result);
        }
    }
}
