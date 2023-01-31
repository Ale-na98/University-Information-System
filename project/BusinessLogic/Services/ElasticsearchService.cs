using AutoMapper;
using BusinessLogic.Domain;
using DataAccess.Elasticsearch;
using System.Collections.Generic;
using DataAccess.Elasticsearch.Documents;

namespace BusinessLogic.Services
{
    public class ElasticsearchService
    {
        private readonly IMapper _mapper;
        private readonly StudentService _studentService;
        private readonly ElasticsearchRepository<StudentDocument> _elasticsearchRepository;

        public ElasticsearchService(ElasticsearchRepository<StudentDocument> elasticsearchRepository,
            StudentService studentService, IMapper mapper)
        {
            _mapper = mapper;
            _studentService = studentService;
            _elasticsearchRepository = elasticsearchRepository;
        }

        public void SaveMany()
        {
            Page<Student> studentPage;
            var pageParams = new PageParams() { CurrentPage = 1, PageSize = 5 };
            do
            {
                studentPage = _studentService.GetAllWithGroups(pageParams);
                var studentDocument = _mapper.Map<IList<StudentDocument>>(studentPage.Data);
                _elasticsearchRepository.SaveMany(studentDocument);
                pageParams.CurrentPage++;
            }
            while (pageParams.CurrentPage <= studentPage.TotalPages);
        }

        public IEnumerable<Student> Search(string query)
        {
            var students = _elasticsearchRepository.Search(query);
            return _mapper.Map<IEnumerable<Student>>(students);
        }

        public void DeleteMany()
        {
            Page<Student> studentPage;
            var pageParams = new PageParams() { CurrentPage = 1, PageSize = 5 };
            do
            {
                studentPage = _studentService.GetAllWithGroups(pageParams);
                var studentDocument = _mapper.Map<IList<StudentDocument>>(studentPage.Data);
                _elasticsearchRepository.DeleteMany(studentDocument);
                pageParams.CurrentPage++;
            }
            while (pageParams.CurrentPage <= studentPage.TotalPages);
        }
    }
}
