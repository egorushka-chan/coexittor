using CoExittor.Common.DTO.Event;
using CoExittor.Common.Models;

namespace CoExittor.Front.Models
{
    public class EventDetailsModel
    {
        public required Event Event { get; init; }
        public ResultDTO? Result { get; init; }
        public string? Error { get; init; }
    }
}
