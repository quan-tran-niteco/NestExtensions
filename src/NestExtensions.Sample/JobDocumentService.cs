using Nest;
using NestExtensions.Sample.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NestExtensions.Sample
{
    public class JobDocumentService
    {
        private IElasticClient m_ElasticClient = ElasticClientFactory.Create(Settings.Alias);

        public async Task Index(Job job)
        {
            await m_ElasticClient.IndexAsync(job.Id, job);
        }

        public async Task Update(Job job)
        {
            await m_ElasticClient.UpdateAsync(job.Id, job);
        }

        public async Task Delete(string id)
        {
            await m_ElasticClient.DeleteAsync<Job>(id);
        }

        public async Task<IEnumerable<Job>> Search(SearchParameters<Job> parameters)
        {
            var response = await m_ElasticClient.SearchAsync(parameters);

            return response.Documents;
        }
    }
}
