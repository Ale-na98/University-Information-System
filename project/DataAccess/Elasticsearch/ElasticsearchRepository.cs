using Nest;
using System.Collections.Generic;

namespace DataAccess.Elasticsearch
{
    public class ElasticsearchRepository<T> where T : class
    {
        private readonly IElasticClient _elasticClient;

        public ElasticsearchRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public void SaveSingle(T document)
        {
            _elasticClient.IndexDocument(document);
        }

        public void SaveMany(IList<T> document)
        {
            _elasticClient.IndexMany(document);
        }

        public IEnumerable<T> Search(string query)
        {
            var response = _elasticClient.Search<T>
            (
                s => s.Query(q => q.QueryString(d => d.Query(query)))
            );
            return response.Documents;
        }

        public void Update(T document)
        {
            _elasticClient.Update<T>(document, u => u.Doc(document));
        }

        public void DeleteSingle(T id)
        {
            _elasticClient.Delete<T>(id);
        }

        public void DeleteMany(IList<T> document)
        {
            _elasticClient.DeleteMany<T>(document);
        }
    }
}
