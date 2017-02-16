using NestExtensions.Aggregations;
using NestExtensions.Queries;
using NestExtensions.Sorts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NestExtensions
{
    public class SearchParameters<TDocument> where TDocument : class
    {
        private int m_Start = 0;
        private int m_Take = 10;

        public IQueryBuilder QueryBuilder { get; set; } = new MatchAllQueryBuilder();

        public IAggregationsBuilder AggregationsBuilder { get; set; }

        public IEnumerable<string> Routings { get; set; } = Enumerable.Empty<string>();

        public int Start
        {
            get
            {
                return m_Start;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(Start));
                }

                m_Start = value;
            }
        }

        public int Take
        {
            get
            {
                return m_Take;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(Take));
                }

                m_Take = value;
            }
        }

        public ICollection<Sort<TDocument>> Sorts { get; set; }
    }
}
