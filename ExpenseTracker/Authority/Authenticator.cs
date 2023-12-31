﻿using System;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;

namespace ExpenseTracker.Authority
{
	public static class Authenticator
	{
        public static bool Authenticate(string clientId, string secret)
        {
            var app = AppRepository.GetApplicationByClientId(clientId);
            if (app == null) return false;

            return app.ClientId == clientId && app.Secret == secret;
        }

        public static string CreateToken(string clientId, DateTime expiresAt, string strSecretKey)
        {
            var app = AppRepository.GetApplicationByClientId(clientId);

            JwtSecurityToken jwt = new JwtSecurityToken();
            try
            {
                var claims = new List<Claim>
            {
                new Claim("AppName", app?.ApplicationName ?? string.Empty),
                new Claim("Read", (app?.Scopes ?? string.Empty).Contains("read") ? "true" : "false"),
                new Claim("Write", (app?.Scopes ?? string.Empty).Contains("write") ? "true" : "false")
            };

                var secretKey = Encoding.ASCII.GetBytes(strSecretKey);

                jwt = new JwtSecurityToken(
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(secretKey),
                        SecurityAlgorithms.HmacSha256Signature),
                        claims: claims,
                        expires: expiresAt,
                        notBefore: DateTime.UtcNow
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public static bool VerifyToken(string token, string strSecretKey)
        {
            if (string.IsNullOrWhiteSpace(token)) return false;

            var secretKey = Encoding.ASCII.GetBytes(strSecretKey);

            SecurityToken securityToken;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero
                },
                out securityToken);
            }
            catch (SecurityTokenException)
            {
                return false;
            }
            catch
            {
                throw;
            }
            return securityToken != null;
        }
    }
}

