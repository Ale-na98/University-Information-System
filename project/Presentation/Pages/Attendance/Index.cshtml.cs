using AutoMapper;
using System.Linq;
using BusinessLogic.Domain;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.DataTransferObjects.Attendance;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Pages.Attendance
{
    public class IndexModel : PageModel
    {
        private readonly AttendanceService _attendanceService;
        private readonly StudentService _studentService;
        private readonly LectureService _lectureService;
        private readonly GroupService _groupService;
        private readonly IMapper _mapper;

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;


        [BindProperty(SupportsGet = true), Display(Name = "Group")]
        public int? GroupId { get; set; }

        [BindProperty(SupportsGet = true)]
        public SearchAttendanceForm SearchAttendanceForm { get; set; } = new SearchAttendanceForm();
        public IList<AttendanceDetailsViewModel> AttendanceDetailsViewModelList { get; set; }
        public Page<BusinessLogic.Domain.Attendance> AttendancePage { get; set; }

        public IEnumerable<SelectListItem> Groups { get; set; }
        public IEnumerable<SelectListItem> Students { get; set; }
        public IEnumerable<SelectListItem> Lectures { get; set; }

        public IndexModel(AttendanceService attendancesService, StudentService studentService,
            LectureService lectureService, GroupService groupService, IMapper mapper)
        {
            _attendanceService = attendancesService;
            _studentService = studentService;
            _lectureService = lectureService;
            _groupService = groupService;
            _mapper = mapper;
        }

        public void OnGet()
        {
            AttendancePage = _attendanceService.GetByFilter
               (
                   GroupId,
                   SearchAttendanceForm.StudentId,
                   SearchAttendanceForm.LectureId,
                   SearchAttendanceForm.DateFrom,
                   SearchAttendanceForm.DateTo,
                   new PageParams() { CurrentPage = CurrentPage, PageSize = PageSize }
               );
            AttendanceDetailsViewModelList = _mapper.Map<IList<AttendanceDetailsViewModel>>(AttendancePage.Data);
            LoadGroupOptions();

            if (GroupId != null)
            {
                LoadOptions();
            }
        }

        public JsonResult OnGetStudents()
        {
            return new JsonResult(_studentService.GetAllByGroupId(GroupId));
        }

        public JsonResult OnGetLectures()
        {
            return new JsonResult(_lectureService.GetAllByGroupId(GroupId));
        }

        private void LoadGroupOptions()
        {
            Groups = _groupService.GetAll()
                    .Select(group => new SelectListItem
                    {
                        Value = group.Id.ToString(),
                        Text = group.Name
                    });
        }

        private void LoadOptions()
        {
            Students = _studentService.GetAllByGroupId(GroupId)
                .Select(student => new SelectListItem
                {
                    Value = student.Id.ToString(),
                    Text = student.FullName
                });

            Lectures = _lectureService.GetAllByGroupId(GroupId)
                .Select(lecture => new SelectListItem
                {
                    Value = lecture.Id.ToString(),
                    Text = lecture.Name
                });
        }
    }
}
