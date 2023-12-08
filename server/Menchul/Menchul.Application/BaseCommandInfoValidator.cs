using Convey.CQRS.Commands;
using FluentValidation;
using Menchul.Convey;

namespace Menchul.Application
{
    /// <summary>
    /// This class helps to unwrap ICommandInfo into a ICommand
    /// </summary>
    /// <typeparam name="TCommand">ICommand</typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class BaseCommandInfoValidator<TCommand, TResult> : AbstractValidator<ICommandInfo<TCommand, TResult>>
        where TCommand : ICommand
    {
        protected BaseCommandInfoValidator(IValidator<TCommand> validator)
        {
            RuleFor(i => i.Command).SetValidator(validator).OverridePropertyName(string.Empty);
        }
    }
}