using System.Net;
using System.Security.Claims;
using CoExittor.Common.DTO.User;
using CoExittor.Common.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoExittor.Front.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> AcceptLogin([FromForm] UserAuthorizationDTO model, string? returnUrl, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", model);
            }

            HttpClient httpClient = _httpClientFactory.CreateClient("BackendClient");

            HttpRequestMessage request = new(HttpMethod.Post, "api/user/login")
            {
                Content = JsonContent.Create(model)
            };

            HttpResponseMessage response = await httpClient.SendAsync(request, token);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Headers.TryGetValues("Set-Cookie", out var setCookies))
                {
                    foreach (var c in setCookies)
                        HttpContext.Response.Headers.Append("Set-Cookie", c);
                }

                if (returnUrl is not null)
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else if(response.StatusCode == HttpStatusCode.BadRequest)
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
                return View("Login", model);
            }
            else
            {
                ModelState.AddModelError("", "Неизвестная ошибка");
                return View("Login", model);
            }
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return View("Login");
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> AcceptRegistration([FromForm] UserRegistrationDTO model, string? returnUrl, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", model);
            }

            HttpClient httpClient = _httpClientFactory.CreateClient("BackendClient");

            HttpRequestMessage request = new(HttpMethod.Post, "api/user/register")
            {
                Content = JsonContent.Create(model)
            };

            HttpResponseMessage response = await httpClient.SendAsync(request, token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (returnUrl is not null)
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
                return View("Login", model);
            }
        }
    }
}
