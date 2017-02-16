using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NestExtensions
{
    public static class ElasticIndexExtensions
    {
        public static async Task<ICreateIndexResponse> CreateIndexAsync(
            this IElasticClient client,
            string indexName,
            IIndexSettings indexSettings = null,
            MappingsDescriptor mappings = null)
        {
            if (string.IsNullOrEmpty(indexName))
            {
                throw new ArgumentNullException(nameof(indexName));
            }

            return await CreateIndexAsync(client, indexSettings, mappings, indexName);
        }

        public static async Task<bool> IsAliasHaveIndicesAsync(this IElasticClient client, string alias)
        {
            var indicesForAlias = await client.GetIndicesPointingToAliasAsync(alias);

            return indicesForAlias.Any();
        }

        public static async Task<IEnumerable<IDeleteIndexResponse>> DeleteIndexByAliasAsync(
            this IElasticClient client, 
            string alias)
        {
            var indicesForAlias = await client.GetIndicesPointingToAliasAsync(alias);
            var responses = new List<IDeleteIndexResponse>(indicesForAlias.Count);

            foreach (var index in indicesForAlias)
            {
                var response = await client.DeleteIndexAsync(index);
                responses.Add(response);
            }

            return responses;
        }

        public static async Task<IBulkAliasResponse> SwapAliasAsync(
            this IElasticClient client, 
            string alias, 
            string indexName)
        {
            if (string.IsNullOrEmpty(alias))
            {
                throw new ArgumentNullException(nameof(alias));
            }

            if (string.IsNullOrEmpty(indexName))
            {
                throw new ArgumentNullException(nameof(indexName));
            }

            var indicesForAlias = client.GetIndicesPointingToAlias(alias);

            var response = await client.AliasAsync(bulkAliasDescriptor =>
            {
                foreach (var index in indicesForAlias)
                {
                    bulkAliasDescriptor = bulkAliasDescriptor
                        .Remove(removeDescriptor => removeDescriptor
                            .Alias(alias)
                            .Index(index));
                }

                bulkAliasDescriptor
                    .Add(addDescriptor => addDescriptor
                        .Alias(alias)
                        .Index(indexName));

                return bulkAliasDescriptor;
            });

            return response;
        }

        public static async Task<string> GetNextIndexNameByAliasAsync(this IElasticClient client, string alias)
        {
            var indexVersionNumber = await GetCurrentIndexNumberAsync(client, alias);

            return $"{alias}_{indexVersionNumber + 1}";
        }

        public static async Task<string> GetLastIndexNameByAliasAsync(this IElasticClient client, string alias)
        {
            var indexVersionNumber = await GetCurrentIndexNumberAsync(client, alias);

            if (indexVersionNumber > 0)
            {
                return $"{alias}_{indexVersionNumber}";
            }

            return null;
        }

        private static async Task<ICreateIndexResponse> CreateIndexAsync(
            IElasticClient client,
            IIndexSettings indexSettings,
            MappingsDescriptor mappings,
            string indexName)
        {
            indexSettings = indexSettings ?? new IndexSettings();
            mappings = mappings ?? new MappingsDescriptor();

            var response = await client.CreateIndexAsync(
                indexName,
                createDescriptor => createDescriptor
                    .Settings(settingDescriptor => settingDescriptor
                        .NumberOfShards(indexSettings.NumberOfShards)
                        .FileSystemStorageImplementation(
                            indexSettings.FileSystemStorageImplementation))
                    .Mappings(mappingsDescriptor => mappings));

            return response;
        }

        private static async Task<int> GetCurrentIndexNumberAsync(IElasticClient client, string alias)
        {
            var indices = await client.GetIndicesPointingToAliasAsync(alias);

            var indexVersionNumber = indices.Count > 0
                ? indices.Max(i =>
                {
                    var version = i.Split('_').Last();
                    return int.Parse(version);
                })
                : 0;

            return indexVersionNumber;
        }
    }
}
