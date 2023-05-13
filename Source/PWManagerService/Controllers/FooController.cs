using Microsoft.AspNetCore.Mvc;
using PWManagerService.Model;

namespace PWManagerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FooController : ControllerBase
    {

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<List<Foo>>> GetAll()
        {

            List<Foo> fooList = new List<Foo>();

            BarOne barOne = new BarOne();
            BarTwo barTwo = new BarTwo();

            fooList.Add(barOne);
            fooList.Add(barTwo);

            return Ok(fooList);
        }

    }
}
