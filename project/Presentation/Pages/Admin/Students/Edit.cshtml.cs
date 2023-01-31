using AutoMapper;
using System.Linq;
using BusinessLogic.Domain;
using BusinessLogic.Services;
using BusinessLogic.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.DataTransferObjects.Students;

namespace Presentation.Pages.Admin.Students
{
    public class EditModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly GroupService _groupService;
        private readonly StudentService _studentService;

        [BindProperty]
        public EditStudentForm EditStudentForm { get; set; }
        public IEnumerable<SelectListItem> Groups { get; set; }

        public EditModel(GroupService groupService, StudentService studentService, IMapper mapper)
        {
            _mapper = mapper;
            _groupService = groupService;
            _studentService = studentService;

            Groups = _groupService.GetAll().Select(group => new SelectListItem
            {
                Value = group.Id.ToString(),
                Text = group.Name
            });
        }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = _studentService.GetWithGroup((int)id);
            EditStudentForm = _mapper.Map<EditStudentForm>(student);

            if (EditStudentForm == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var student = _mapper.Map<Student>(EditStudentForm);
                _studentService.Update(student.Id, student);
            }
            catch (StudentNotFoundException)
            {
                return base.NotFound();
            }

            return RedirectToPage("./Index");
        }
    }
}
