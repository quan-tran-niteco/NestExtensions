using Nest;
using System;
using System.Linq.Expressions;

namespace NestExtensions.Queries
{
    public class TermQueryBuilder<TDocument> : IQueryBuilder
        where TDocument : class
    {
        public Expression<Func<TDocument, object>> FieldPath { get; set; }

        public object Value { get; set; }

        public double? Boost { get; set; }

        public QueryContainer Build()
        {
            return new TermQuery()
            {
                Field = FieldPath.ToPathString(),
                Boost = this.Boost,
                Value = this.Value
            };
        }
    }
}
