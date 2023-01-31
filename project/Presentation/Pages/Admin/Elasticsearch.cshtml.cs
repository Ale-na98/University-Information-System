using AutoMapper;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin
{
    public class ElasticsearchModel : PageModel
    {
        private readonly ElasticsearchService _elasticsearchService;

        public ElasticsearchModel(ElasticsearchService elasticsearchService, IMapper mapper)
        {
            _elasticsearchService = elasticsearchService;
        }

        public void OnPostSave()
        {
            _elasticsearchService.SaveMany();
        }

        public void OnPostReindex()
        {
            _elasticsearchService.DeleteMany();
            _elasticsearchService.SaveMany();
        }

        public void OnPostDelete()
        {
            _elasticsearchService.DeleteMany();
        }
    }
}
