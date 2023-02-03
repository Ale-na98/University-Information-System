using AutoMapper;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin
{
    public class ElasticsearchModel : PageModel
    {
        private readonly string _studentIndexName = "students";
        private readonly ElasticsearchService _elasticsearchService;

        public ElasticsearchModel(ElasticsearchService elasticsearchService, IMapper mapper)
        {
            _elasticsearchService = elasticsearchService;
        }

        public void OnPostCreateStudentIndex()
        {
            _elasticsearchService.CreateStudentIndex(_studentIndexName);
        }

        public void OnPostRecreateStudentIndex()
        {
            _elasticsearchService.DeleteIndex(_studentIndexName);
            _elasticsearchService.CreateStudentIndex(_studentIndexName);
        }

        public void OnPostDeleteStudentIndex()
        {
            _elasticsearchService.DeleteIndex(_studentIndexName);
        }
    }
}
