using LearningTracker.Api.Models;
using LearningTracker.Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;

using System.Security.Claims;

namespace LearningTracker.Tests;

public class JwtTokenServiceTests
{
    [Fact]
    public void CreateToken_ValidUser_ReturnsTokenWithThreeParts()
    {
        var settings = new Dictionary<string, string?>
        {
            ["Jwt:Key"] = "dfhadfhslkdfjhlskdlfkjhsdfkjhk;sdkl;fglkskdfkjk;l",
            ["Jwt:Issuer"] = "TestIssuer",
            ["Jwt:Audience"] = "TestAudience",
            ["Jwt:ExpiresMinutes"] = "15",
        };
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        var JwtService = new JwtTokenService(configuration);

        var token = JwtService.CreateToken(new User { Id = 1, Email = "test@mail.ru", Role = "User" });

        Assert.Equal(3, token.Split('.').Length);
    }

    [Fact]
    public void CreateToken_ValidUser_ContainsUserClaims()
    {
        var settings = new Dictionary<string, string?>
        {
            ["Jwt:Key"] = "dlkgsldjgslkdjgjsdklgjsjdglskdjgjskdjgjlsdlgkskdjgjsldkgjskl",
            ["Jwt:Issuer"] = "TestIssuer",
            ["Jwt:Audience"] = "TestAudience",
            ["Jwt:ExpiresMinutes"] = "15",
        };
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        var jwtService = new JwtTokenService(configuration);

        var token = jwtService.CreateToken(new User { Id = 1, Email = "test@mail.ru", Role = "User" });

        var json = new JsonWebTokenHandler();

        var obj = json.ReadJsonWebToken(token);

        Assert.Equal("test@mail.ru", obj.GetClaim(ClaimTypes.Email).Value.ToString());
        Assert.Equal(1, Convert.ToInt32(obj.GetClaim(ClaimTypes.NameIdentifier).Value));
        Assert.Equal("User", obj.GetClaim(ClaimTypes.Role).Value.ToString());
    }

    [Fact]
    public void CreateToken_NoKeyInConfiguration_ThrowsInvalidOperationException()
    {
        var settings = new Dictionary<string, string?>
        {
            ["Jwt:Issuer"] = "TestIssuer",
            ["Jwt:Audience"] = "TestAudience",
            ["Jwt:ExpiresMinutes"] = "15",
        };
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        var jwtService = new JwtTokenService(configuration);

        Assert.Throws<InvalidOperationException>(() => jwtService.CreateToken(new User { Id = 1, Email = "test@mail.ru", Role = "User" }));
    }

    [Fact]
    public void CreateToken_ExpiresMinutes15_TokenLivesFor15Minutes()
    {
        var settings = new Dictionary<string, string?>
        {
            ["Jwt:Key"] = "dlkgsldjgslkdjgjsdklgjsjdglskdjgjskdjgjlsdlgkskdjgjsldkgjskl",
            ["Jwt:Issuer"] = "TestIssuer",
            ["Jwt:Audience"] = "TestAudience",
            ["Jwt:ExpiresMinutes"] = "15",
        };
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        var jwtService = new JwtTokenService(configuration);

        var token = jwtService.CreateToken(new User { Id = 1, Email = "test@mail.ru", Role = "User" });

        var json = new JsonWebTokenHandler();

        var obj = json.ReadJsonWebToken(token);

        Assert.Equal(900, (obj.ValidTo - obj.ValidFrom).TotalSeconds);
    }
}
