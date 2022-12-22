using Microsoft.AspNetCore.Mvc;

namespace Observe.OpenTelemetry.AppA.Controllers
{
    public class PersonController : Controller
    {
        private readonly HttpClient _httpClient;

        public PersonController(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("ApiB");
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result =await _httpClient.GetStringAsync("GetAllA");
            return Ok(result);
        }
    }
}
