using Nest;
using NestExtensions.Sorts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NestExtensions
{
    public static class ElasticSearchExtensions
    {
        public static async Task<ISearchResponse<T>> SearchAsync<T>(
            this IElasticClient client, 
            SearchParameters<T> parameters) 
            where T : class
        {
            var searchRequest = parameters.ToSearchRequest();

            return await client.SearchAsync<T>(searchRequest);
        }

        internal static ISearchRequest ToSearchRequest<TDocument>(
            this SearchParameters<TDocument> parameters) 
            where TDocument : class
        {
            var searchDescriptor = new SearchDescriptor<TDocument>()
                .From(parameters.Start)
                .Size(parameters.Take)
                .Query(queryDescriptor => parameters.QueryBuilder.Build());

            if (parameters.Sorts?.Any() == true)
            {
                searchDescriptor = searchDescriptor.Sort(sortDescriptor => parameters.Sorts.ToSortDescriptor());
            }

            if (parameters.Routings?.Any() == true)
            {
                searchDescriptor = searchDescriptor.Routing(parameters.Routings.ToArray());
            }

            if (parameters.AggregationsBuilder != null)
            {
                searchDescriptor = searchDescriptor.Aggregations(
                    aggregationContainerDescriptor => parameters.AggregationsBuilder.Build());
            }

            return searchDescriptor;
        }

        internal static SortDescriptor<TDocument> ToSortDescriptor<TDocument>(
            this IEnumerable<Sort<TDocument>> sorts)
            where TDocument : class
        {
            var sortDescriptor = new SortDescriptor<TDocument>();

            foreach (var sort in sorts)
            {
                sortDescriptor = sort.DescribeTo(sortDescriptor);
            }

            return sortDescriptor;
        } 
    }
}
