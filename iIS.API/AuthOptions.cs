using Microsoft.IdentityModel.Tokens;
using System.Text;

public class AuthOptions
{
    public const string ISSUER = "iISServer";
    public const string AUDIENCE = "iISClient";
    private const string KEY = "mysupersecret_secretsecretsecretkey!123";

    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}