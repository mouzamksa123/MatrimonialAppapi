using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using matrimonial.Models;
using matrimonial.Service;

namespace matrimonial.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly TokenService tokenService;
        private readonly IOptions<AppSettingsModel> appSettings;

        public AccountController(IOptions<AppSettingsModel> appsetting)
        {
            appSettings = appsetting;
            tokenService = new TokenService(appsetting);
        }
        /// <summary>
        /// Generate an Access token
        /// </summary>
        /// <param name="userLogins"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> GetToken([FromBody] UserLogins userLogins)
        {
            try
            {
                User user=null;

                if (userLogins.UserName== "test".ToString().ToLower() && userLogins.UserName == "test".ToString().ToLower())
                {
                    user = new User();
                }

                if (user != null && !string.IsNullOrWhiteSpace(user.UserName))
                {
                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.EmailId),
                    new Claim(ClaimTypes.GivenName, user.DisplayName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                    var token = tokenService.CreateToken(authClaims);
                    var refreshToken = tokenService.GenerateRefreshToken();

                    _ = int.TryParse(appSettings.Value.JWT.RefreshTokenValidityInDays, out int refreshTokenValidityInDays);

                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);


                    return Ok(new
                    {
                        access_token = new JwtSecurityTokenHandler().WriteToken(token),
                        refresh_token = refreshToken,
                        expiration = token.ValidTo
                    });
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        [Route("refreshtoken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null || tokenModel.access_token is null)
            {
                return BadRequest("Invalid client request");
            }

            string? accessToken = tokenModel.access_token;
            string? refreshToken = tokenModel.refresh_token;

            var principal = tokenService.GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var newAccessToken = tokenService.CreateToken(principal.Claims.ToList());
            var newRefreshToken = tokenService.GenerateRefreshToken();

            return new ObjectResult(new
            {
                access_token = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refresh_token = newRefreshToken,
                expiration = newAccessToken.ValidTo
            });
        }

        /// <summary>
        /// Get List of UserAccounts   
        /// </summary>
        /// <returns>List Of UserAccounts</returns>
        [HttpGet]
        public IActionResult GetList()
        {
            return Ok();
        }
        [HttpPost]
        [Route("logout")]
        public IActionResult logout()
        {
            return Ok();
        }
    }
}
