using Nest;
using System;
using System.Configuration;

namespace NestExtensions.Sample
{
    public static class ElasticClientFactory
    {
        private static string _ConnectionUri = ConfigurationManager.AppSettings["ElasticUri"];

        public static IElasticClient Create(string defaultIndex = null)
        {
            var uri = new Uri(_ConnectionUri);

            var setting = new ConnectionSettings(uri);
            if (!string.IsNullOrEmpty(defaultIndex))
            {
                setting.DefaultIndex(defaultIndex);
            }

            var client = new ElasticClient(setting);

            return client;
        }
    }
}
