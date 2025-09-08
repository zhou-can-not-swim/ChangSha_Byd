using Microsoft.AspNetCore.Mvc;

namespace ChangSha_Byd_NetCore8.Controllers
{
    [Route("api/")]
    [ApiController]
    public class TestHomeController : Controller
    {
        [HttpGet("test")]
        public String Load()
        {
            return "Hello World!";
        }
    }
}
