using Convey.CQRS.Commands;

namespace Menchul.Convey
{
    /// <summary>
    /// For command handlers use the interface <see cref="ICommandInfo{TCommand,TResponse}"/>
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class CommandInfo<TCommand, TResponse> : ICommandInfo<TCommand, TResponse> where TCommand : ICommand
    {
        public TCommand Command { get; }
        public TResponse Result { get; set; }

        public CommandInfo(TCommand command) => Command = command;
    }
}