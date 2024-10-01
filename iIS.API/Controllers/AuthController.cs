using FluentValidation.Results;
using iIS.API.Contracts;
using iIS.API.Validation;
using iIS.Core.Errors;
using iIS.Core.Models;
using iIS.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
                DateOnly birthDate = DateOnly.Parse(request.BirthDate);
                await _usersService.Register(request.UserName, birthDate, request.Email, request.Password);
                return Results.Created();
            }
            catch(ExistUserException ex)
            {
                return Results.Conflict(ex.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("session")]
        public async Task<IResult> Login([FromBody] LoginRequest request)
        {
            var validator = new LoginRequestValidator();
            ValidationResult validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
                return Results.BadRequest(validationResult.Errors);

            try
            {
                User user = await _usersService.Login(request.LoginType, request.Login, request.Password);

                var claims = new List<Claim>
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim(ClaimTypes.DateOfBirth, user.BirthDay.ToString())
                };

                var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.Now.AddSeconds(30),
                    signingCredentials: new
                                SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                                SecurityAlgorithms.HmacSha256));

                string token = new JwtSecurityTokenHandler().WriteToken(jwt);
                Response.Cookies.Append("auth-key", token);

                return Results.Ok();
            }
            catch (NotFoundUserException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (WrongPasswordException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}