using JsonSerialization.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JsonSerialization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferenceTypesController : ControllerBase
    {
        [HttpGet("single-open-generic")]
        public IActionResult Get()
        {
            var data = new DummyData();
            data.String= "   Test ";
            return Ok(data);
        }

        [HttpPost("single-open-generic")]
        public IActionResult SingleGeneric(DummyData dummyData)
        {
            return Ok(dummyData);
        }

        [HttpPost("single-open-generic1")]
        public IActionResult SingleGeneric1(DummyData dummyData)
        {
            return Ok(dummyData);
        }

        [HttpPut("single-open-generic")]
        public IActionResult SingleGeneric1p(DummyData dummyData)
        {
            return Ok(dummyData);
        }

        [HttpPost("list-open-generic")]
        public IActionResult ListOfGeneric(List<DummyData> dummyDatas)
        {
            return Ok(dummyDatas);
        }
    }
}
