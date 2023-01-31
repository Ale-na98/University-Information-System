using AutoMapper;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.DataTransferObjects.Attendance;

namespace Presentation.Pages.Attendance
{
    public class DeleteModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly AttendanceService _attendanceService;

        [BindProperty]
        public AttendanceDetailsViewModel AttendanceDetailsViewModel { get; set; }

        public DeleteModel(AttendanceService attendanceService, IMapper mapper)
        {
            _mapper = mapper;
            _attendanceService = attendanceService;
        }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = _attendanceService.GetWithLecture((int)id);
            AttendanceDetailsViewModel = _mapper.Map<AttendanceDetailsViewModel>(attendance);

            if (AttendanceDetailsViewModel == null)
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

            if (_attendanceService.GetWithLecture((int)id) != null)
            {
                _attendanceService.Delete((int)id);
            }

            return RedirectToPage("./Index");
        }
    }
}
