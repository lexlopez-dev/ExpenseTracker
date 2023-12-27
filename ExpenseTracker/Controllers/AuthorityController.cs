﻿using System;
using ExpenseTracker.Authority;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
	[ApiController]
	public class AuthorityController : ControllerBase
	{

        [HttpPost("auth")]
		public IActionResult Authenticate([FromBody]AppCredential credential)
		{
			if (AppRepository.Authenticate(credential.ClientId, credential.Secret))
			{
				return Ok(new
				{
					access_token = CreateToken(credential.ClientId),
					expires_at = DateTime.UtcNow.AddMinutes(10)
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

        private object CreateToken(string clientId)
        {
            throw new NotImplementedException();
        }
    }
}

