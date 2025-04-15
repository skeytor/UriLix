using Microsoft.Extensions.DependencyInjection;
using UriLix.Shared.Results;

namespace UriLix.Application.Abstractions.Messaging;

public class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
{
    public async Task<Result<TResult>> DispatchAsync<TCommand, TResult>(
        TCommand command, 
        CancellationToken cancellationToken = default) where TCommand : ICommand<TResult>
    {
        using var scope = serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>()
            ?? throw new InvalidOperationException($"No handler registered for command type {typeof(TCommand).Name}");
        return await handler.HandleAsync(command, cancellationToken);
    }
}
