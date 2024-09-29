using Microsoft.AspNetCore.Mvc;

namespace iIS.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IResult GetUserData()
        {
            return Results.Ok("User data");
        }
    }
}