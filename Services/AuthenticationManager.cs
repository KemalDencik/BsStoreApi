using AutoMapper;
using Entities.DataTransferObject;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthenticationManager : IAuthenticationService
    {
        private readonly ILoggerService _logger;//loglanan ifade için kullanılacak
        private readonly IMapper _mapper;//dto dan user a dönerken mapper kullanılacak
        private readonly UserManager<User> _userManager; //kullanıcı kaydı yaparken kullanılacak 
        private readonly IConfiguration _configuration;//konfigürasyon için kullanılacak

        private User? _user; //user a ait bilgileri içerecek
        public AuthenticationManager(ILoggerService logger, IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<TokenDto> CreateToken(bool populateExp)
        {
            var singinCredentials = GetSigninCredentials(); //kullanıcı bilgileri
            var claims = await GetClaims();//kullanıcı hangi rolde alınır
            var tokenOptions = GenerateTokenOptions(singinCredentials, claims); //token oluşturma opt lerini gnerate ettik

            var refreshToken = GenerateRefreshToken();
            _user.RefreshToken = refreshToken;

            //doğruysa süre uzat 
            if(populateExp)
                _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await _userManager.UpdateAsync(_user);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);//ilgili tokenin oluşmasını sağladık 
            return new TokenDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public  async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistrationDto)
        {
            var user = _mapper.Map<User>(userForRegistrationDto); 

            var result = await _userManager.CreateAsync(user, userForRegistrationDto.Password);

            if(result.Succeeded)//başarılımı
                await _userManager.AddToRolesAsync(user, userForRegistrationDto.Roles);
            
            return result;
        }

        public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuthDto)
        {
            _user = await _userManager.FindByNameAsync(userForAuthDto.UserName);
            var result = (_user != null && await _userManager.CheckPasswordAsync(_user, userForAuthDto.Password));
            if (!result )
            {
                _logger.LogWarning($"{nameof(ValidateUser)} : Authentication failed. wrong username or password");
            }
            return result;
        }
        private SigningCredentials GetSigninCredentials()
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["secretKey"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret,SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,_user.UserName)
            };//liste tanımı oluşturma

            var roles = await _userManager
                .GetRolesAsync(_user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials singinCredentials,
            List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");//ilgili section ı appset den aldık 

            //nesne ürettik ilgili nesneyi döndük   
            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
                signingCredentials: singinCredentials);
            return tokenOptions;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        //kullanıcı bilgilerini alacağım
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["secretKey"];
            //doğrulama
            var tokenValidationParameters = new TokenValidationParameters
            {
                //doğrulama parametrelerimiz
                ValidateIssuer = true,//keyi kullananı doğrula
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["validIssuer"],
                ValidAudience = jwtSettings["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            };

            var tokenHnadler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            //doğrulama işlemini gerçeklerştirdik
            //out kısmı değişken ataması gibi bir tanım var metod çalıştıktan sonra security token ın değeri ilgili alanda set edilecek
            var principal = tokenHnadler.ValidateToken(token, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken is null ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase)) 
            {
                throw new SecurityTokenException("Invalid Token");
            }
            return principal;
        }

        public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            //veri tabanına gidip tokenı ben ürettim sana soruyorum kullanıcı varmı dicem
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);

            //user veritabanında olmayabilir control yapıyorum
            if (user is null ||
                user.RefreshToken != tokenDto.RefreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.Now)
                throw new RefreshTokenBadRequestException();

            _user = user;
            return await CreateToken(populateExp: false);
        }
    }
}
