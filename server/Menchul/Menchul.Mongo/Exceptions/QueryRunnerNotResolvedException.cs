using Menchul.Application.Exceptions;
using Menchul.Mongo.QueryRunners;

namespace Menchul.Mongo.Exceptions
{
    public class QueryRunnerNotResolvedException<T> : AppException
    {
        public QueryRunnerNotResolvedException(QueryRunnerType? key) : base(
            $"A query runner [{key}] can't be resolved for type [{typeof(T).Name}]")
        {
        }
    }
}