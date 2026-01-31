using CoExittor.Api.Application.Services.Interfaces;
using CoExittor.Api.Domain.Repositories;
using CoExittor.Common.DTO.Event;
using CoExittor.Common.Models;

namespace CoExittor.Api.Application.Services
{
    internal class EventService : IEventService
    {
        private readonly IEventRepository _repository;

        public EventService(IEventRepository repository)
        {
            _repository = repository;
        }

        public Task<ResultDTO> CalculateResult(string eventCode, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<string> CreateEvent(CreateEventDTO createEventDTO, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<List<Event>> GetAllEventsAsync(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<Event?> GetEventByCode(string code, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task ParticipateInEvent(string eventCode, ParticipateEventDTO participateEventDTO, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
