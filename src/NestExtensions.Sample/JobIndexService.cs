using Nest;
using NestExtensions.Sample.Models;
using System.Threading.Tasks;

namespace NestExtensions.Sample
{
    public class JobIndexService
    {
        private IElasticClient m_ElasticClient = ElasticClientFactory.Create();

        public async Task CreateAsync()
        {
            var indexName = await m_ElasticClient.GetNextIndexNameByAliasAsync(Settings.Alias);
            var createResponse = await m_ElasticClient.CreateIndexAsync(
                indexName, 
                mappings: new MappingsDescriptor().Map<Job>(m => m
                    .Properties(p => p
                        .String(s => s
                            .NotAnalyzed()
                            .Name(j => j.Id)))
                    .Properties(p => p
                        .String(s => s
                            .Analyzer("ik")
                            .SearchAnalyzer("ik_smart")
                            .Name(j => j.Name)))
                    .Properties(p => p
                        .Number(n => n
                            .Name(j => j.Type)))
                    .Properties(p => p
                        .String(s => s
                            .NotAnalyzed()
                            .Name(j => j.Contact)))));

            if (createResponse.Acknowledged)
            {
                await m_ElasticClient.SwapAliasAsync(Settings.Alias, indexName);
            }
        }

        public async Task DeleteAsync()
        {
            await m_ElasticClient.DeleteIndexByAliasAsync(Settings.Alias);
        }

        public async Task RefreshAsync()
        {
            await m_ElasticClient.RefreshAsync(Settings.Alias);
        }
    }
}
