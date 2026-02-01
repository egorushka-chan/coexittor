using CoExittor.Common.DTO.Message;

namespace CoExittor.Api.Domain.Exceptions.Interfaces
{
    public interface IMessageException
    {
        DefaultErrorMessage ToErrorMessage();
    }
}
