using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace iIS.API.Auth.Requirements
{
    public class AgeHandler : AuthorizationHandler<AgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeRequirement requirement)
        {
            Claim? birthClaim = context.User.FindFirst(ClaimTypes.DateOfBirth);
            if(birthClaim == null)
            {
                context.Fail(new AuthorizationFailureReason(this, "Нет claim`а `DateOfBirth`"));
                return Task.CompletedTask;
            }

            DateOnly birthDate = DateOnly.Parse(birthClaim.Value);
            DateTime now = DateTime.Now;
            now = now.AddYears(-birthDate.Year);
            now = now.AddMonths(-birthDate.Month);
            now = now.AddDays(-birthDate.Day);

            if(now.Year >= requirement.Age)
                context.Succeed(requirement);
            else
                context.Fail(new AuthorizationFailureReason(this, $"Возраст: {now.Year}, а должен быть {requirement.Age}"));

            return Task.CompletedTask;
        }
    }
}