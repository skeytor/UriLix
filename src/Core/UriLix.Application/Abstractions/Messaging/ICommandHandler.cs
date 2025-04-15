using UriLix.Shared.Results;

namespace UriLix.Application.Abstractions.Messaging;

internal interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    Task<Result> HandleAsync(
        TCommand command,
        CancellationToken cancellationToken);
}

internal interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse>> HandleAsync(
        TCommand command,
        CancellationToken cancellationToken);
}