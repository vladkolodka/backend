using Convey;
using Convey.Logging.CQRS;
using Menchul.Application.Exceptions;
using Menchul.Convey;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Menchul.Infrastructure.Logging
{
    internal sealed class SimpleMessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
        private static readonly string CommandInfoTypeName = typeof(CommandInfo<,>).Name;

        // this method also can be used to customize log messages for specific commands, including command values
        public HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class
        {
            var messageType = message.GetType();

            var typeName = messageType.Name == CommandInfoTypeName
                ? messageType.GenericTypeArguments.First().Name
                : messageType.Name;
            var name = typeName.Underscore();;

            var commonTemplate = new HandlerLogTemplate
            {
                Before = $"Handling a command: '{name}'...",
                After = $"Handled command: '{name}'.",
                OnError = new Dictionary<Type, string>
                {
                    {typeof(AppValidationException), $"Invalid command request '{name}'"}
                }
            };

            return commonTemplate;
        }
    }
}