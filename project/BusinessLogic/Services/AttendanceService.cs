using System;
using AutoMapper;
using DataAccess;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace BusinessLogic
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IHometasksRepository _hometasksRepository;
        private readonly ILecturesRepository _lecturesRepository;
        private readonly ILogger<AttendanceService> _logger;
        private readonly IEmailProvider _emailProvider;
        private readonly ISmsProvider _smsProvider;
        private readonly IMapper _mapper;

        public AttendanceService(IAttendanceRepository attendanceRepository, IHometasksRepository hometasksRepository, ILecturesRepository lecturesRepository, 
            IEmailProvider emailProvider, ISmsProvider smsProvider, ILogger<AttendanceService> logger, IMapper mapper)
        {
            _attendanceRepository = attendanceRepository;
            _hometasksRepository = hometasksRepository;
            _lecturesRepository = lecturesRepository;
            _emailProvider = emailProvider;
            _smsProvider = smsProvider;
            _logger = logger;
            _mapper = mapper;
        }

        public void SetPresence(AttendanceDto attendanceDto)
        {
            var attendanceDb = _mapper.Map<AttendanceDb>(attendanceDto);
            _attendanceRepository.Create(attendanceDb);
            _logger.LogInformation($"The presence on the lecture {attendanceDto.LectureId} of the student {attendanceDto.StudentId} for {attendanceDto.LectureDate} was set.");            

            if (attendanceDb.Presence == false || attendanceDb.HometaskDone == false)
            {
                var hometask = new HometaskDb()
                {
                    HometaskDate = DateTime.Now,
                    Mark = 0,
                    StudentId = attendanceDto.StudentId,
                    LectureId = attendanceDto.LectureId
                };
                _hometasksRepository.Create(hometask);
                _logger.LogInformation($"The hometask for the student {attendanceDto.StudentId} and the lecture {attendanceDto.LectureId} with the mark 0 was created.");
            }
            var teacherId = _lecturesRepository.Get(attendanceDb.LectureId).TeacherId;
            _emailProvider.SendEmail(attendanceDb.LectureId, teacherId, attendanceDb.StudentId);
            _smsProvider.SendSms(attendanceDb.LectureId, attendanceDb.StudentId);
        }

        public AttendanceDto Get(int lectureId, int studentId, DateTime lectureDate)
        {
            var attendanceDb = _attendanceRepository.Get(lectureId, studentId, lectureDate);
            if (attendanceDb == null)
            {
                throw new AttendanceNotFoundException($"There is no attendance for the lecture {lectureId} and the student {studentId} for {lectureDate}.");
            }
            return _mapper.Map<AttendanceDto>(attendanceDb);
        }

        public IReadOnlyCollection<StudentAttendanceDto> CreateLectureAttendanceReport(string lectureName)
        {
            var attendance = _attendanceRepository.GetLectureAttendance(lectureName);
            if (!attendance.Any())
            {
                _logger.LogWarning($"There is no any record of attendance at the lecture {lectureName}.");
                return new List<StudentAttendanceDto>();
            }
            var attendanceGroupedByStudent = attendance.GroupBy(a => a.Student).ToDictionary(a => a.Key, a => a.ToList());
            var attendanceList = new List<StudentAttendanceDto>();
            foreach (var a in attendanceGroupedByStudent)
            {
                var student = a.Key;
                var studentDto = new StudentAttendanceDto()
                {
                    FullName = student.FullName,
                    Attendance = _mapper.Map<List<AttendanceReportDto>>(a.Value)
                };
                attendanceList.Add(studentDto);
            }
            return attendanceList;
        }

        public IReadOnlyCollection<LectureAttendanceDto> CreateStudentAttendanceReport(string studentFullName)
        {
            var attendance = _attendanceRepository.GetStudentAttendance(studentFullName);
            if (attendance.GetEnumerator().MoveNext() == false)
            {
                _logger.LogWarning($"There is no any record of attendance of the student {studentFullName}.");
                return new List<LectureAttendanceDto>();
            }
            var attendanceGroupedByLecture = attendance.GroupBy(a => a.Lecture).ToDictionary(a => a.Key, a => a.ToList());
            var attendanceList = new List<LectureAttendanceDto>();
            foreach (var a in attendanceGroupedByLecture)
            {
                var lecture = a.Key;
                var lectureDto = new LectureAttendanceDto()
                {
                    Name = lecture.Name,
                    Attendance = _mapper.Map<List<AttendanceReportDto>>(a.Value)
                };
                attendanceList.Add(lectureDto);
            }
            return attendanceList;
        }
    }
}