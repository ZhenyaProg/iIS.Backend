using FluentValidation.Results;
using iIS.API.Contracts;
using iIS.API.Validation;
using iIS.Core.Auth;
using iIS.Core.Errors;
using iIS.Core.Models;
using iIS.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace iIS.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly ITokenProvider _tokenProvider;
        private readonly ILogger<UserController> _logger;

        public UserController(IUsersService usersService, ITokenProvider tokenProvider, ILogger<UserController> logger)
        {
            _usersService = usersService;
            _tokenProvider = tokenProvider;
            _logger = logger;
        }

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

        [HttpPut]
        [Route("")]
        [Authorize(Policy = nameof(Policy.User))]
        public async Task<IResult> EditUserData([FromBody] EditUserRequest request, [FromHeader] Guid userId)
        {
            var validator = new EditRequestValidator();
            ValidationResult validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
                return Results.BadRequest(validationResult.Errors);

            try
            {
                DateOnly birthDate = DateOnly.Parse(request.BirthDate);
                User editData = new User
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    BirthDay = birthDate,
                };
                User user = await _usersService.EditUser(userId, editData);

                string token = _tokenProvider.GenerateToken(user);
                Response.Cookies.Append("auth-key", token);

                return Results.Ok();
            }
            catch (NotFoundUserException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = nameof(Policy.User))]
        public async Task<IResult> DeleteUser([FromHeader] Guid userId)
        {
            try
            {
                await _usersService.DeleteUser(userId);
                Response.Cookies.Delete("auth-key");
                return Results.Ok();
            }
            catch (NotFoundUserException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}