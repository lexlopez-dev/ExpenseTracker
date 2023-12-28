﻿using System;
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

        public AuthorityController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost("auth")]
		public IActionResult Authenticate([FromBody]AppCredential credential)
		{
            var expiresAt = DateTime.UtcNow.AddMinutes(10);

            if (Authenticator.Authenticate(credential.ClientId, credential.Secret))
			{
				return Ok(new
				{
					access_token = Authenticator.CreateToken(credential.ClientId, expiresAt, configuration.GetValue<string>("SecretKey")),
					expires_at = expiresAt
                });
            }
            else
			{
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

