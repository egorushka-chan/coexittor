using System.Net;
using System.Text.Json;
using CoExittor.Common.DTO.Event;
using CoExittor.Common.DTO.Message;
using CoExittor.Common.Models;
using CoExittor.Front.Services.Interfaces;

namespace CoExittor.Front.Services
{
    public class BackendClient : IBackendClient
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

        private readonly HttpClient _http;

        public BackendClient(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient("BackendClient");
        }

        public async Task<List<Event>> GetAllEventsAsync(CancellationToken token)
        {
            using var resp = await _http.GetAsync("api/event/all", token);
            await EnsureSuccessOrThrow(resp, token);
            return (await resp.Content.ReadFromJsonAsync<List<Event>>(JsonOptions, token)) ?? [];
        }

        public async Task<Event> GetEventByCodeAsync(Guid eventCode, CancellationToken token)
        {
            using var resp = await _http.GetAsync($"api/event/by-code/{eventCode}", token);
            await EnsureSuccessOrThrow(resp, token);
            var model = await resp.Content.ReadFromJsonAsync<Event>(JsonOptions, token);
            return model ?? throw new InvalidOperationException("Empty response body.");
        }

        public async Task<Guid> CreateEventAsync(CreateEventDTO dto, CancellationToken token)
        {
            using var resp = await _http.PostAsJsonAsync("api/event/create", dto, JsonOptions, token);

            if (resp.StatusCode == HttpStatusCode.Created)
            {
                // Location: .../api/event/by-code/{eventCode}
                var location = resp.Headers.Location?.ToString();
                if (!string.IsNullOrWhiteSpace(location))
                {
                    var last = location.Split('/', StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                    if (Guid.TryParse(last, out var code))
                        return code;
                }
                return Guid.Empty;
            }

            await EnsureSuccessOrThrow(resp, token);
            return Guid.Empty;
        }

        public async Task ParticipateAsync(Guid eventCode, ParticipateEventDTO dto, CancellationToken token)
        {
            using var resp = await _http.PostAsJsonAsync($"api/event/participate/{eventCode}", dto, JsonOptions, token);
            await EnsureSuccessOrThrow(resp, token);
        }

        public async Task<ResultDTO> CalculateAsync(Guid eventCode, CancellationToken token)
        {
            using var resp = await _http.GetAsync($"api/event/calculate/{eventCode}", token);
            await EnsureSuccessOrThrow(resp, token);
            return (await resp.Content.ReadFromJsonAsync<ResultDTO>(JsonOptions, token)) ?? new ResultDTO();
        }

        public async Task AcceptAsync(Guid eventCode, CancellationToken token)
        {
            using var resp = await _http.PostAsync($"api/event/accept/{eventCode}", content: null, token);
            await EnsureSuccessOrThrow(resp, token);
        }

        private static async Task EnsureSuccessOrThrow(HttpResponseMessage response, CancellationToken token)
        {
            if (response.IsSuccessStatusCode)
                return;

            DefaultErrorMessage? apiError = null;
            try
            {
                apiError = await response.Content.ReadFromJsonAsync<DefaultErrorMessage>(JsonOptions, token);
            }
            catch
            {
            }

            var details = apiError is null
                ? $"HTTP {(int)response.StatusCode} ({response.ReasonPhrase})"
                : $"{apiError.Title} (HTTP {apiError.Status})\n{apiError.Details}";

            throw new InvalidOperationException(details);
        }
    }
}
