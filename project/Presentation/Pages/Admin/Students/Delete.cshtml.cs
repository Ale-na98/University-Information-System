using AutoMapper;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.DataTransferObjects.Students;

namespace Presentation.Pages.Admin.Students
{
    public class DeleteModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly StudentService _studentService;

        public DeleteModel(StudentService studentService, IMapper mapper)
        {
            _mapper = mapper;
            _studentService = studentService;
        }

        [BindProperty]
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

        public IActionResult OnPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (_studentService.GetWithGroup((int)id) != null)
            {
                _studentService.Delete((int)id);
            }

            return RedirectToPage("./Index");
        }
    }
}
