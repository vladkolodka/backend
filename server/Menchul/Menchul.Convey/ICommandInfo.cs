using Convey.CQRS.Commands;

namespace Menchul.Convey
{
    public interface ICommandInfo<out TCommand, TResponse> : ICommand where TCommand : ICommand
    {
        public TCommand Command { get; }

        // TODO setter?
        public TResponse Result { get; set; }
    }
}