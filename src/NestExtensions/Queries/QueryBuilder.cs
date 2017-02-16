using Nest;
using System;

namespace NestExtensions.Queries
{
    public abstract class QueryBuilder<TQueryParameters> : IQueryBuilder<TQueryParameters>
    {
        private TQueryParameters m_QueryParameters;

        protected QueryBuilder(TQueryParameters queryParameters)
        {
            QueryParameters = queryParameters;
        } 

        public TQueryParameters QueryParameters
        {
            get
            {
                return m_QueryParameters;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("QueryParameters");
                }

                m_QueryParameters = value;
            }
        }

        public abstract QueryContainer Build();
    }
}
