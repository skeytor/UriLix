using UriLix.Shared.Results;

namespace UriLix.Application.Abstractions.Messaging;

public interface IDispatcher
{
    Task<Result<TResult>> DispatchAsync<TCommand, TResult>(
        TCommand command, 
        CancellationToken cancellationToken = default)
    where TCommand : ICommand<TResult>;
}
