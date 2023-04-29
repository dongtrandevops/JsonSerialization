using JsonSerialization.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace JsonSerialization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> TestAsync(DummyData test)
        {
            if (test.Pet.FieldStatus.ContainsKey(nameof(DummyData.Pet.Name)))
            {
                var ok = 1;
            }
            return Ok(test);
        }

        [HttpPatch]
        public async Task<IActionResult> PatchAsync([FromBody] JsonPatchDocument<DummyData> patchDoc)
        {
            var data = new DummyData();
            patchDoc.ApplyTo(data);

            return Ok(data);
        }


        [HttpPost("list")]
        public async Task<IActionResult> ListAsync(IEnumerable<Person> tests)
        {
            return Ok();
        }

        [HttpPost("dictionary")]
        public async Task<IActionResult> DictionaryAsync(Dictionary<WeekType, string> test)
        {
            return Ok(test);
        }
        

    }
}
