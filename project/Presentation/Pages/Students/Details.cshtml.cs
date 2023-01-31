using AutoMapper;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.DataTransferObjects.Students;

namespace Presentation.Pages.Students
{
    public class DetailsModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly StudentService _studentService;

        public DetailsModel(StudentService studentService, IMapper mapper)
        {
            _mapper = mapper;
            _studentService = studentService;
        }

        public StudentDetailsViewModel StudentDetailsViewModel { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = _studentService.GetWithGroup((int)id);
            StudentDetailsViewModel = _mapper.Map<StudentDetailsViewModel>(student);

            if (StudentDetailsViewModel == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
