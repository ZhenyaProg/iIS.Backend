using FluentValidation.Results;
using iIS.API.Contracts;
using iIS.API.Validation;
using iIS.Core.Auth;
using iIS.Core.Errors;
using iIS.Core.Models;
using iIS.Core.Services;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ITokenProvider _tokenProvider;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUsersService usersService, ITokenProvider tokenProvider, ILogger<AuthController> logger)
        {
            _usersService = usersService;
            _tokenProvider = tokenProvider;
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
                User registeredUser = new User
                {
                    UserName = request.UserName,
                    BirthDay = birthDate,
                    Email = request.Email,
                };
                await _usersService.Register(registeredUser, request.Password);
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
        public async Task<IResult> LogIn([FromBody] LoginRequest request)
        {
            var validator = new LoginRequestValidator();
            ValidationResult validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
                return Results.BadRequest(validationResult.Errors);

            try
            {
                User user = await _usersService.LogIn(request.LoginType, request.Login, request.Password);

                string token = _tokenProvider.GenerateToken(user);
                Response.Cookies.Append("auth-key", token);

                return Results.Ok(user.Id);
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

        [HttpDelete]
        [Route("session")]
        [Authorize(Policy = nameof(Policy.User))]
        public async Task<IResult> LogOut([FromHeader] Guid userId)
        {
            try
            {
                await _usersService.LogOut(userId);
                Response.Cookies.Delete("auth-key");
                return Results.Ok();
            }
            catch (NotFoundUserException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}