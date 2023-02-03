using BusinessLogic.Domain;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.DataTransferObjects.Students;

namespace Presentation.Pages
{
    public class SearchModel : PageModel
    {
        private readonly ElasticsearchService _elasticsearchService;

        public StudentDetailsViewModel StudentDetailsViewModel { get; set; }

        [BindProperty]
        public string Query { get; set; }
        public IEnumerable<Student> Documents { get; set; } = new List<Student>();

        public SearchModel(ElasticsearchService elasticsearchService)
        {
            _elasticsearchService = elasticsearchService;
        }

        public void OnPostSearchStudents()
        {
            Documents = _elasticsearchService.SearchStudents(Query);
        }
    }
}
