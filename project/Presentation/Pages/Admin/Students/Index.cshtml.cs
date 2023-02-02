using AutoMapper;
using BusinessLogic.Domain;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.DataTransferObjects.Students;

namespace Presentation.Pages.Admin.Students
{
    public class IndexModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly StudentService _studentService;
        private readonly ElasticsearchService _elasticsearchService;

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 9;

        [BindProperty(SupportsGet = true)]
        public SearchStudentForm SearchStudentForm { get; set; } = new SearchStudentForm();
        public IList<StudentDetailsViewModel> StudentDetailsViewModelList { get; set; }
        public Page<Student> StudentPage { get; set; }

        public IndexModel(StudentService studentService, IMapper mapper, ElasticsearchService elasticsearchService)
        {
            _mapper = mapper;
            _studentService = studentService;
            _elasticsearchService = elasticsearchService;
        }

        public void OnGet()
        {
            StudentPage = _studentService.GetByFilter
                (
                    SearchStudentForm.Id,
                    SearchStudentForm.FullName,
                    SearchStudentForm.Email,
                    SearchStudentForm.PhoneNumber,
                    SearchStudentForm.Group,
                    new PageParams() { CurrentPage = CurrentPage, PageSize = PageSize }
                );

            StudentDetailsViewModelList = _mapper.Map<IList<StudentDetailsViewModel>>(StudentPage.Data);
        }
    }
}
