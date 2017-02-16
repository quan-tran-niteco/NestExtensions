using Nest;
using NestExtensions.Queries;
using NestExtensions.Sample.Models;

namespace NestExtensions.Sample
{
    public class JobQueryBuilder : QueryBuilder<string>
    {
        public JobQueryBuilder(string query) : base(query)
        { }

        public override QueryContainer Build()
        {
            return new QueryContainerDescriptor<Job>()
                .Match(matchDescriptor => matchDescriptor
                    .Operator(Operator.And)
                    .Query(QueryParameters)
                    .Field(j => j.Name));
        }
    }
}
