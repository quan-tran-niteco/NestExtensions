using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NestExtensions.Queries
{
    public class CompositeQueryBuilder : IQueryBuilder
    {
        public ICollection<IQueryBuilder> QueryBuilders { get; set; }

        public Operator Operator { get; set; } = Operator.And;

        public QueryContainer Build()
        {
            if (QueryBuilders?.Any() != true)
            {
                throw new InvalidOperationException("QueryBuilders is null or empty");
            }

            QueryContainer queryContainer = null;

            foreach (var queryBuilder in QueryBuilders)
            {
                var query = queryBuilder.Build();

                if (queryContainer == null)
                {
                    queryContainer = query;
                }
                else
                {
                    if (Operator == Operator.And)
                    {
                        queryContainer = queryContainer && query;
                    }
                    else
                    {
                        queryContainer = queryContainer || query;
                    }
                }
            }

            return queryContainer;
        }
    }
}
