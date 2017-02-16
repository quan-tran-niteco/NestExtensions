using Nest;

namespace NestExtensions.Queries
{
    public class MatchAllQueryBuilder : IQueryBuilder
    {
        public QueryContainer Build()
        {
            return new MatchAllQuery();
        }
    }
}
