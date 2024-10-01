using Microsoft.AspNetCore.Authorization;

namespace iIS.API.Auth.Requirements
{
    public class AgeRequirement : IAuthorizationRequirement
    {
        protected internal int Age { get; init; }
        public AgeRequirement(int age) => Age = age;
    }
}