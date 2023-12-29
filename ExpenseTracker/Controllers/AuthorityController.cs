using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExpenseTracker.Authority;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ExpenseTracker.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
	public class AuthorityController : ControllerBase
	{
        private readonly IConfiguration configuration;
        private readonly ILogger<AuthorityController> _logger;

        public AuthorityController(IConfiguration configuration, ILogger<AuthorityController> logger)
        {
            this.configuration = configuration;
            _logger = logger;
        }

        [HttpPost("auth")]
		public IActionResult Authenticate([FromBody]AppCredential credential)
		{
            var expiresAt = DateTime.UtcNow.AddMinutes(10);

            if (Authenticator.Authenticate(credential.ClientId, credential.Secret))
			{
                _logger.LogInformation("Authenticator.Authenticate successful");

                string accessToken = "";
                try
                {
                    accessToken = Authenticator.CreateToken(credential.ClientId, expiresAt, configuration.GetValue<string>("SecretKey"));
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Authenticator.CreateToken failed ex: {ex}");
                }
                
                _logger.LogInformation($"Authenticator.CreateToken successful : {accessToken}");
                return Ok(new
				{
					access_token = accessToken,
					expires_at = expiresAt
                });
            }
            else
			{
                _logger.LogError("Error Authenticating");

                ModelState.AddModelError("Unauthorized", "You are not authorized.");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status401Unauthorized
                };
                return new UnauthorizedObjectResult(problemDetails);
            }
		}
    }
}

