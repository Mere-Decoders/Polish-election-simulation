using Microsoft.AspNetCore.Mvc;
using backend.Services.Methods;
namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MethodTestController : ControllerBase
    {
        private readonly MethodTest _service;

        public MethodTestController(MethodTest service)
        {
            _service = service;
        }

        [HttpGet]
        public string Get()
        {
            return _service.TestMethod();
        }
    }
}
