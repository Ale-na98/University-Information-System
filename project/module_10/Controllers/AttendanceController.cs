using System;
using System.Linq;
using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace module_10
{
    [ApiController]
    [Route("/api/attendance")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpPost]
        public ActionResult<string> SetPresence(AttendanceDto attendanceDto)
        {
            _attendanceService.SetPresence(attendanceDto);
            return Ok($"The presence was set.");
        }

        [HttpGet]
        public ActionResult<AttendanceDto> Get([BindRequired] int lectureId, [BindRequired] int studentId, [BindRequired] DateTime lectureDate)
        {
            return Ok(_attendanceService.Get(lectureId, studentId, lectureDate));
        }

        [HttpGet("/api/attendance/reports/lecture/{lectureName}.{format}"), FormatFilter]
        public ActionResult<IReadOnlyCollection<StudentAttendanceDto>> GetLectureAttendanceReport(string lectureName)
        {
            return Ok(_attendanceService.CreateLectureAttendanceReport(lectureName));
        }

        [HttpGet("/api/attendance/reports/student/{studentFullName}.{format}"), FormatFilter]
        public ActionResult<IReadOnlyCollection<LectureAttendanceDto>> GetStudentAttendanceReport(string studentFullName)
        {
            return Ok(_attendanceService.CreateStudentAttendanceReport(studentFullName));
        }
    }
}