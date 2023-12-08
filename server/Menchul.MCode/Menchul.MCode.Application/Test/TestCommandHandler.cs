using Convey.CQRS.Commands;
using Menchul.Convey;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Menchul.MCode.Application.Test
{
    public class TestCommandHandler : ICommandHandler<ICommandInfo<TestCommand, string>>
    {
        /// <inheritdoc />
        private Task<string> HandleAsync(TestCommand command)
        {
            return Task.FromResult("This is working");
        }

        public async Task HandleAsync(ICommandInfo<TestCommand, string> info)
        {
            var a = new JObject();


            info.Result = await HandleAsync(info.Command);
        }
    }
}