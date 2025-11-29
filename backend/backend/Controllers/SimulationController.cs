using backend.Data;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using backend.Services.Methods;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SimulationController : ControllerBase
    {
        private readonly ISimulationService _simulationService;

        public SimulationController(ISimulationService simulationService)
        {
            _simulationService = simulationService;
        }
        /// <summary>
        /// This method runs a simulation on raw Data and a counting method identified by their GUIDs.
        /// </summary>
        /// <param name="simDataGuid"> Parameter identifying the raw election dataset. The empty guid(00000000-0000-0000-0000-000000000000) will serve a sane debug value.</param> 
        /// <param name="methodGuid">Parameter identifying the counting method to apply to the dataset. The empty guid(00000000-0000-0000-0000-000000000000) will serve a sane debug value.</param>
        /// <returns>ElectionResult - serializable model of the result of an election.</returns>
        [HttpGet(Name = "simulate")]
        public ElectionResult Simulate(Guid simDataGuid, Guid methodGuid)
        {
            return _simulationService.Simulate(simDataGuid, methodGuid);
        }
    }
}
