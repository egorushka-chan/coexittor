using System.Net;
using CoExittor.Common.DTO.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoExittor.Front.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _http;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient("BackendClient");
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

            HttpRequestMessage request = new(HttpMethod.Post, "api/user/login")
            {
                Content = JsonContent.Create(model)
            };

            HttpResponseMessage response = await _http.SendAsync(request, token);
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
            return Redirect("Login");
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

            HttpRequestMessage request = new(HttpMethod.Post, "api/user/register")
            {
                Content = JsonContent.Create(model)
            };

            HttpResponseMessage response = await _http.SendAsync(request, token);

            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
            {
                // Backend издаёт cookie, возращаем его браузеру
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
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ModelState.AddModelError("", "Не удалось зарегистрироваться. Проверьте данные (возможно, email уже занят).");
                return View("Register", model);
            }
            else
            {
                ModelState.AddModelError("", "Неизвестная ошибка при регистрации.");
                return View("Register", model);
            }
        }
    }
}
