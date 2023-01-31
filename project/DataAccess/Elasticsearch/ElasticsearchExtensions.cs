using Nest;
using System;
using Microsoft.Extensions.Configuration;
using DataAccess.Elasticsearch.Documents;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Elasticsearch
{
    public static class ElasticsearchExtensions
    {
        public static IServiceCollection AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["elasticsearch:url"];
            var defaultIndex = configuration["elasticsearch:index"];

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex);

            IElasticClient client = new ElasticClient(settings);

            CreateIndex(client, defaultIndex);

            return services.AddSingleton(client);
        }

        private static void CreateIndex(IElasticClient client, string indexName)
        {
            client.Indices.Create(indexName, index => index.Map<StudentDocument>(x => x.AutoMap()));
        }
    }
}
