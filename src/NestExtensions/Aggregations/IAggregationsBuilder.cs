using Nest;

namespace NestExtensions.Aggregations
{
    public interface IAggregationsBuilder
    {
        IAggregationContainer Build();
    }
}
