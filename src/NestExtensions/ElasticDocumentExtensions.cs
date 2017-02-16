using NestExtensions.Bulk;
using Nest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NestExtensions
{
    public static class ElasticDocumentExtensions
    {
        public static async Task<IGetResponse<T>> GetAsync<T>(
            this IElasticClient client,
            string id,
            string routing = null,
            string indexName = null)
            where T : class
        {
            indexName = indexName ?? client.ConnectionSettings.DefaultIndex;
            routing = routing ?? id;

            return await client.GetAsync<T>(id, getDescriptor => getDescriptor.Routing(routing).Index(indexName));
        }

        public static async Task<IIndexResponse> IndexAsync<T>(
            this IElasticClient client,
            string id,
            T document,
            string routing = null,
            string indexName = null)
            where T : class
        {
            indexName = indexName ?? client.ConnectionSettings.DefaultIndex;
            routing = routing ?? id;

            return await client.IndexAsync(
                document, indexDerscriptor => indexDerscriptor.Routing(routing).Id(id).Index(indexName));
        }

        public static async Task<IDeleteResponse> DeleteAsync<T>(
            this IElasticClient client,
            string id,
            string routing = null,
            string indexName = null)
            where T : class
        {
            indexName = indexName ?? client.ConnectionSettings.DefaultIndex;
            routing = routing ?? id;

            return await client.DeleteAsync<T>(id, deleteDescriptor => deleteDescriptor.Routing(routing).Index(indexName));
        }

        public static async Task<IUpdateResponse<T>> UpdateAsync<T>(
            this IElasticClient client,
            string id,
            T document,
            string routing = null,
            string indexName = null)
            where T : class
        {
            indexName = indexName ?? client.ConnectionSettings.DefaultIndex;
            routing = routing ?? id;

            return await client.UpdateAsync<T>(
                id, updateDescriptor => updateDescriptor.Routing(routing).Index(indexName).Doc(document));
        }

        public static async Task<IBulkResponse> BulkAsync<T>(
            this IElasticClient client,
            IEnumerable<BulkItem<T>> items,
            string routing = null,
            string indexName = null)
            where T : class
        {
            indexName = indexName ?? client.ConnectionSettings.DefaultIndex;

            var response = await client.BulkAsync(bulkDescriptor =>
            {
                bulkDescriptor = bulkDescriptor.Index(indexName);
                if (routing != null)
                {
                    bulkDescriptor = bulkDescriptor.Routing(routing);
                }

                foreach (var item in items)
                {
                    switch (item.Operation)
                    {
                        case BulkOperation.Index:
                            bulkDescriptor = bulkDescriptor.Index<T>(
                                indexDescriptor => indexDescriptor
                                    .Routing(item.Routing)
                                    .Document(item.Document)
                                    .Id(item.Id));
                            break;
                        case BulkOperation.Create:
                            bulkDescriptor = bulkDescriptor.Create<T>(
                                createDescriptor => createDescriptor
                                    .Routing(item.Routing)
                                    .Document(item.Document)
                                    .Id(item.Id));
                            break;
                        case BulkOperation.Delete:
                            bulkDescriptor = bulkDescriptor.Delete<T>(
                                deleteDescriptor => deleteDescriptor
                                    .Routing(item.Routing)
                                    .Id(item.Id));
                            break;
                        case BulkOperation.Upsert:
                            bulkDescriptor = bulkDescriptor.Update<T>(
                                updateDescriptor => updateDescriptor
                                    .Routing(item.Routing)
                                    .Doc(item.Document)
                                    .DocAsUpsert(true)
                                    .Id(item.Id));
                            break;
                    }
                }

                return bulkDescriptor;
            });

            return response;
        }
    }
}
