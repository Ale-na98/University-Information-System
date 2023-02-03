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
        private readonly ElasticsearchRepository<StudentDocument> _studentElasticsearchRepository;

        public ElasticsearchService(ElasticsearchRepository<StudentDocument> studentElasticsearchRepository,
            StudentService studentService, IMapper mapper)
        {
            _mapper = mapper;
            _studentService = studentService;
            _studentElasticsearchRepository = studentElasticsearchRepository;
        }

        public void CreateStudentIndex(string indexName)
        {
            _studentElasticsearchRepository.CreateIndex(indexName);
            SaveManyStudents();
        }

        public void DeleteIndex(string indexName)
        {
            _studentElasticsearchRepository.DeleteIndex(indexName);
        }

        public void SaveManyStudents()
        {
            Page<Student> studentPage;
            var pageParams = new PageParams() { CurrentPage = 1, PageSize = 5 };
            do
            {
                studentPage = _studentService.GetAllWithGroups(pageParams);
                var studentDocument = _mapper.Map<IList<StudentDocument>>(studentPage.Data);
                _studentElasticsearchRepository.SaveMany(studentDocument);
                pageParams.CurrentPage++;
            }
            while (pageParams.CurrentPage <= studentPage.TotalPages);
        }

        public IEnumerable<Student> SearchStudents(string query)
        {
            var students = _studentElasticsearchRepository.Search(query);
            return _mapper.Map<IEnumerable<Student>>(students);
        }
    }
}
