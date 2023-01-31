using BusinessLogic.Domain;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.DataTransferObjects.Students;
using System.Collections.Generic;

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

        public void OnPost()
        {
            Documents = _elasticsearchService.Search(Query);
        }
    }
}
