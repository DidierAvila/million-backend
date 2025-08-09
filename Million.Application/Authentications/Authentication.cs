using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Million.Domain.DTOs;
using Million.Domain.Entities;
using Million.Domain.Repositories;

namespace Million.Application.Authentications
{
    public class Authentication : IAuthentication
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenRepository _TokenRepository;
        private readonly IUserRepository _UserRepository;
        private readonly ILogger<Authentication> _logger;

        public Authentication(IConfiguration configuration, ITokenRepository tokenRepository, IUserRepository userRepository, ILogger<Authentication> logger)
        {
            _configuration = configuration;
            _TokenRepository = tokenRepository;
            _UserRepository = userRepository;
            _logger = logger;
        }

        public async Task<LoginResponse> Login(LoginRequest autorizacion, CancellationToken cancellationToken)
        {
            var CurrentUser = await _UserRepository.FindAsync(x => x.Email == autorizacion.UserName && x.Password == autorizacion.Password, cancellationToken);
            if (CurrentUser.Any())
            {
                _logger.LogInformation("Login: succes");
                string CurrentToken = await GetToken(CurrentUser.First(), cancellationToken);
                return new LoginResponse() { Token = CurrentToken, Success = true };
            }
            return new LoginResponse() { Success = false, Messages = "Login failed for user:" + autorizacion.UserName };
        }

        private async Task<string> GenerateTokenAsync(User user, CancellationToken cancellationToken)
        {
            string? key = _configuration.GetValue<string>("JwtSettings:key");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Crear los claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.Name),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, user.Role),
            };

            // Crear el token
            DateTime ExperiredDate = DateTime.Now.AddMinutes(60);
            JwtSecurityToken tokenJwt = new(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: ExperiredDate,
                signingCredentials: credentials
            );

            string Newtoken = new JwtSecurityTokenHandler().WriteToken(tokenJwt);

            //Se almacena el nuevo token
            if (!string.IsNullOrEmpty(Newtoken))
            {
                Token token = new()
                {
                    TokenValue = Newtoken,
                    Status = true,
                    CreatedDate = DateTime.Now,
                    ExpirationDate = ExperiredDate
                };
                _logger.LogInformation("GetToken: Expiration Token UserId: {UserId}", user.Id);
                await _TokenRepository.AddAsync(token, cancellationToken);
            }
            return Newtoken;
        }

        private async Task<string> RefreshTokenAsync(Token token, User user, CancellationToken cancellationToken)
        {
            token.Status = false;
            await _TokenRepository.UpdateAsync(token.Id, token, cancellationToken);

            string currentToken = await GenerateTokenAsync(user, cancellationToken);
            return currentToken;
        }

        private async Task<string> GetToken(User user, CancellationToken cancellationToken)
        {
            var CurrentToken = await _TokenRepository.FindAsync(x => x.UserId == user.Id && x.Status, cancellationToken);
            if (CurrentToken.Any())
            {
                var Token = CurrentToken.First();
                if (Token.ExpirationDate.CompareTo(DateTime.Now) < 0)
                {
                    _logger.LogInformation("GetToken: Expiration Token UserId: {UserId}", user.Id);
                    return await RefreshTokenAsync(Token, user, cancellationToken);
                }
                return Token.TokenValue!;
            }
            else
            {
                string currentToken = await GenerateTokenAsync(user, cancellationToken);
                if (!string.IsNullOrEmpty(currentToken))
                {
                    return currentToken;
                }
            }
            return string.Empty;
        }
    }
}
