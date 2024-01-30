using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MockDB;

namespace FinalProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public IActionResult List()
        {
            return Ok(Users.GetUsers());
        }
    }
}
