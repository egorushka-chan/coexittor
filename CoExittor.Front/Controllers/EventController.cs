using System.Reflection;
using System.Security.Claims;
using CoExittor.Common.DTO.Event;
using CoExittor.Common.DTO.Voting;
using CoExittor.Common.Models;
using CoExittor.Front.Models;
using CoExittor.Front.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoExittor.Front.Controllers
{
    [Route("events")]
    public class EventController : Controller
    {
        private readonly IBackendClient _apiClient;

        public EventController(IBackendClient apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View(new CreateEventModel());
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventModel model, CancellationToken token)
        {
            if (!ModelState.IsValid)
                return View(model);

            string? userIDStr = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            long? userID = userIDStr is null ? null : long.Parse(userIDStr);

            var dto = new CreateEventDTO
            {
                Name = model.Name,
                Description = model.Description,
                Host = new CreateEventDTO.HostDTO
                {
                    Name = model.HostName,
                    LinkedUserID = userID,
                    Votings = [.. model.Votings
                        .Where(v => v.StartDate != default && v.EndDate != default)
                        .Select(v => new VotingDTO { StartDate = v.StartDate, EndDate = v.EndDate })]
                }
            };

            try
            {
                var code = await _apiClient.CreateEventAsync(dto, token);
                if (code == Guid.Empty)
                {
                    TempData["FlashError"] = "Событие создано, но не удалось извлечь код из ответа!";
                    return RedirectToAction(nameof(Index), "Home");
                }

                TempData["FlashSuccess"] = "Событие создано!";
                return RedirectToAction(nameof(Details), new { code = code });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpGet("details")]
        public async Task<IActionResult> Details([FromQuery] Guid code, CancellationToken token)
        {
            try
            {
                var ev = await _apiClient.GetEventByCodeAsync(code, token);
                return View(new EventDetailsModel { Event = ev });
            }
            catch (InvalidOperationException ex)
            {
                return View(new EventDetailsModel
                {
                    Event = new Event { Code = code, Name = "Не найдено" },
                    Error = ex.Message
                });
            }
        }

        [HttpGet("join")]
        public async Task<IActionResult> Join([FromQuery] Guid code, CancellationToken token)
        {
            try
            {
                var ev = await _apiClient.GetEventByCodeAsync(code, token);
                ViewBag.EventName = ev.Name;
            }
            catch
            {
                ViewBag.EventName = null;
            }

            return View(new ParticipateEventModel { EventCode = code });
        }

        [HttpPost("join")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join([FromQuery] Guid code, ParticipateEventModel model, CancellationToken token)
        {
            model.EventCode = code;

            if(model.IsAgreedWithDefault is false && model.Votings.Count == 0)
            {
                TempData["FlashError"] = "Если вы не согласны с датами создателя, введите собственные";
                return View(model);
            }


            if (!ModelState.IsValid)
                return View(model);

            var dto = new ParticipateEventDTO
            {
                Name = model.Name,
                IsAgreedWithDefault = model.IsAgreedWithDefault,
                LinkedUserID = null,
                Votings = model.Votings
                    .Where(v => v.StartDate != default && v.EndDate != default)
                    .Select(v => new VotingDTO { StartDate = v.StartDate, EndDate = v.EndDate })
                    .ToList()
            };

            try
            {
                await _apiClient.ParticipateAsync(code, dto, token);
                TempData["FlashSuccess"] = "Вы добавлены в событие.";
                return RedirectToAction(nameof(Details), new { code = code });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpPost("accept")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept([FromQuery] Guid code, CancellationToken token)
        {
            try
            {
                await _apiClient.AcceptAsync(code, token);
                TempData["FlashSuccess"] = "Событие принято (закреплено).";
            }
            catch (InvalidOperationException ex)
            {
                TempData["FlashError"] = ex.Message;
            }

            return RedirectToAction(nameof(Details), new { code = code });
        }

        [HttpGet("result")]
        public async Task<IActionResult> Result([FromQuery] Guid code, CancellationToken token)
        {
            try
            {
                var ev = await _apiClient.GetEventByCodeAsync(code, token);
                var res = await _apiClient.CalculateAsync(code, token);
                return View(new EventDetailsModel { Event = ev, Result = res });
            }
            catch (InvalidOperationException ex)
            {
                var ev = new Event { Code = code, Name = "Не найдено" };
                return View(new EventDetailsModel { Event = ev, Error = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> List(CancellationToken token)
        {
            try
            {
                List<Event> list = await _apiClient.GetAllEventsAsync(token);

                return View(list);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
    }
}
