using iIS.Core.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace iIS.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("info")]
        [Authorize(Policy = nameof(Policy.User))]
        public IResult GetUserData()
        {
            if(User.Identity is ClaimsIdentity identity)
            {
                DateOnly birthDate = DateOnly.Parse(identity.FindFirst(ClaimTypes.DateOfBirth)?.Value);
                DateTime now = DateTime.Now;
                now = now.AddYears(-birthDate.Year);
                now = now.AddMonths(-birthDate.Month);
                now = now.AddDays(-birthDate.Day);
                var userData = new
                {
                    id = identity.FindFirst("Id")?.Value,
                    userName = identity.FindFirst(ClaimTypes.Name)?.Value,
                    email = identity.FindFirst(ClaimTypes.Email)?.Value,
                    role = identity.FindFirst(ClaimTypes.Role)?.Value,
                    age = now.Year
                };

                return Results.Ok(userData);
            }
            return Results.BadRequest();
        }
    }
}