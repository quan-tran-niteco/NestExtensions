using Nest;

namespace NestExtensions.Queries
{
    public class NoMatchQueryBuilder : IQueryBuilder
    {
        public QueryContainer Build()
        {
            return new NoMatchQueryContainer();
        }
    }
}
