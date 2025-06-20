﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shop.Enum;
using Shop.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Shop.Attribute
{
    public class AuthorizeRoleAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly UserRole[] _roles;

        public AuthorizeRoleAttribute(params UserRole[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //var user = context.HttpContext.User;
            var token = context.HttpContext.Request.Cookies["test-cookie"];
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new ForbidResult();
            }

            // Парсим токен
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Извлекаем значение из claim с типом Sid
            var roleClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;


            if (roleClaim != null && UserRole.TryParse<UserRole>(roleClaim, out var userRole) && _roles.Contains(userRole))
            {
                return;
            }

            context.Result = new ForbidResult();
        }
    }
}
