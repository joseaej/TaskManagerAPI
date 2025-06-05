using System.Security.AccessControl;
using System.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskFlowAPI.Services;
using TasksManagerAPI.Data;
using TasksManagerAPI.Models.Api;
using TasksManagerAPI.Models.Entity;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace TasksManagerAPI.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dbcontext;
        public JwtService(AppDbContext dbContext, IConfiguration configuration) {
            _dbcontext = dbContext;
            _configuration = configuration;
        }

        public async Task<TokenResponseModel?> Autenticate(AccesTokenModel request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
            {
                return null;   
            }
            var userAccount = await _dbcontext.Accounts.FirstOrDefaultAsync(Account => Account.PasswordHash == request.Password);
            if (userAccount is null || !CryptographyService.CheckPassword(request.Password,userAccount.PasswordHash)) { 

            }

            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];
            var tokenvalidatemins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
            var tokenExpireTime = DateTime.UtcNow.AddMinutes(tokenvalidatemins);

            var tokendescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Name ,request.UserName),
                }),
                Expires = tokenExpireTime,
                Issuer = issuer,    
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                SecurityAlgorithms.HmacSha256Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokendescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return new TokenResponseModel
            {
                AccessToken = accessToken,
                UserName = request.UserName,
                ExpiresIn = (int)tokenExpireTime.Subtract(DateTime.UtcNow).TotalSeconds,
            };
        }
    }
}
