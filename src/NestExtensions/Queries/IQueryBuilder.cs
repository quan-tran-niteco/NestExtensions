using Nest;

namespace NestExtensions.Queries
{
    public interface IQueryBuilder
    {
        QueryContainer Build();
    }

    public interface IQueryBuilder<TQueryParameters> : IQueryBuilder
    {
        TQueryParameters QueryParameters { get; set; }
    }
}
