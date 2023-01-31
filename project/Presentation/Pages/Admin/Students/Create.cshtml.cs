using AutoMapper;
using System.Linq;
using BusinessLogic.Domain;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.DataTransferObjects.Students;

namespace Presentation.Pages.Admin.Students
{
    public class CreateModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly GroupService _groupService;
        private readonly StudentService _studentService;

        [BindProperty]
        public CreateStudentForm CreateStudentForm { get; set; }
        public IEnumerable<SelectListItem> Groups { get; set; }

        public CreateModel(GroupService groupsService, StudentService studentsService, IMapper mapper)
        {
            _mapper = mapper;
            _groupService = groupsService;
            _studentService = studentsService;

            Groups = _groupService.GetAll().Select(group => new SelectListItem
            {
                Value = group.Id.ToString(),
                Text = group.Name
            });
        }

        public IActionResult OnGet() => Page();

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _studentService.Create(_mapper.Map<Student>(CreateStudentForm));

            return RedirectToPage("./Index");
        }
    }
}
