namespace UriLix.Application.Abstractions.Messaging;

public interface ICommand : IBaseCommand;
public interface ICommand<TResult> : IBaseCommand;
public interface IBaseCommand;
