using FluentValidation.Results;
using iIS.API.Contracts;
using iIS.API.Validation;
using iIS.Core.Errors;
using iIS.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace iIS.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUsersService usersService, ILogger<AuthController> logger)
        {
            _usersService = usersService;
            _logger = logger;
        }

        [HttpPost]
        [Route("user")]
        public async Task<IResult> Register([FromBody] CreateUserRequest request)
        {
            var validator = new CreateUserRequestValidator();
            ValidationResult validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
                return Results.BadRequest(validationResult.Errors);

            try
            {
                await _usersService.Register(request.UserName, request.Email, request.Password);
                return Results.Created();
            }
            catch(ExistUserException cuException)
            {
                return Results.Conflict(cuException.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}