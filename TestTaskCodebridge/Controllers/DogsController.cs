using Microsoft.AspNetCore.Mvc;
using TestTaskCodebridge.DTOs;
using TestTaskCodebridge.Services;

namespace TestTaskCodebridge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DogsController : ControllerBase
    {
        public readonly DogsService _dogsService;

        public DogsController(DogsService dogsService)
        {
            _dogsService = dogsService;
        }


        [HttpGet("ping")]
        public IActionResult GetPing()
        {
            return Ok();
        }


        [HttpGet("dogs")]
        public async Task<IActionResult> GetAllDogs([FromQuery] string attribute = "name", 
                                                    [FromQuery] string order = "asc", 
                                                    [FromQuery] int pageNumber = 1, 
                                                    [FromQuery] int pageSize = 10)
        {
            return Ok(await _dogsService.GetAllDogsAsync(attribute, order, pageNumber, pageSize));
        }


        [HttpPost("dog")]
        public async Task<IActionResult> AddNewDog(CreateDogDTO createDogDTO)
        {
            return Ok(await _dogsService.CreateNewDogAsync(createDogDTO));
        }
    }
}
