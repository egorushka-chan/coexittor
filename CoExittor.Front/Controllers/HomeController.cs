using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoExittor.Front.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OpenEvent(string code)
        {
            if (Guid.TryParse(code, out var guid))
                return RedirectToAction("Details", "Events", new { eventCode = guid });

            TempData["FlashError"] = "Некорректный код (ожидается GUID).";
            return RedirectToAction(nameof(Index));
        }
    }
}
