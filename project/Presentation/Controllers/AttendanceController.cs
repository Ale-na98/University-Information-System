using System;
using AutoMapper;
using BusinessLogic.Domain;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Presentation.DataTransferObjects.Attendance;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("/api/attendance")]
    public class AttendanceController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AttendanceService _attendanceService;

        public AttendanceController(AttendanceService attendanceService, IMapper mapper)
        {
            _mapper = mapper;
            _attendanceService = attendanceService;
        }

        [HttpGet("{id}")]
        public ActionResult<AttendanceDetailsViewModel> GetWithLecture(int id)
        {
            var attendance = _attendanceService.GetWithLecture(id);
            return Ok(_mapper.Map<AttendanceDetailsViewModel>(attendance));
        }

        [HttpGet("attendance.{format}"), FormatFilter]
        public ActionResult<IList<AttendanceDetailsViewModel>> GetByFilter(int? groupId, int? studentId, int? lectureId,
            DateTime? lectureDateFrom, DateTime? lectureDateTo, [FromQuery] PageParams parameters)
        {
            var attendance = _attendanceService.GetByFilter(groupId, studentId, lectureId, lectureDateFrom,
                lectureDateTo, parameters);
            return Ok(_mapper.Map<IList<AttendanceDetailsViewModel>>(attendance.Data));
        }

        [HttpGet("reports/lecture/{lectureId}.{format}"), FormatFilter]
        public ActionResult<IList<AttendanceDetailsViewModel>> GetLectureAttendanceReport(int lectureId, [FromQuery] PageParams parameters)
        {
            var attendance = _attendanceService.GetLectureAttendanceReport(lectureId, parameters);
            return Ok(_mapper.Map<IList<AttendanceDetailsViewModel>>(attendance.Data));
        }

        [HttpGet("reports/student/{studentId}.{format}"), FormatFilter]
        public ActionResult<IList<AttendanceDetailsViewModel>> GetStudentAttendanceReport(int studentId, [FromQuery] PageParams parameters)
        {
            var attendance = _attendanceService.GetStudentAttendanceReport(studentId, parameters);
            return Ok(_mapper.Map<IList<AttendanceDetailsViewModel>>(attendance.Data));
        }
    }
}
