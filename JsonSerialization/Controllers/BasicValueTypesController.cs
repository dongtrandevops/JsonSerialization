using JsonSerialization.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JsonSerialization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasicValueTypesController : ControllerBase
    {
        [HttpPost("string")]
        public async Task<IActionResult> StringAsync(DummyData dummyData)
        {
            return Ok(dummyData);
        }
    }
}
