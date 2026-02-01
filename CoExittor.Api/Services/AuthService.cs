using System.Security.Claims;
using CoExittor.Common.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CoExittor.Api.Services
{
    public class AuthService
    {
        public static async Task IssueCookie(User user, HttpContext context)
        {
            List<Claim> claims = 
                [
                new(ClaimTypes.Name, user.Name), 
                new(ClaimTypes.Email, user.Email), 
                new (ClaimTypes.NameIdentifier, user.ID.ToString())
                ];
            ClaimsIdentity claimsIdentity = new(claims, "Cookies");
            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }
    }
}
