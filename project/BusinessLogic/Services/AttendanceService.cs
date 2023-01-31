using System;
using AutoMapper;
using System.Linq;
using DataAccess.Entities;
using BusinessLogic.Domain;
using DataAccess.Interfaces;
using BusinessLogic.Providers;
using BusinessLogic.Exceptions;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Services
{
    public class AttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IHometaskRepository _hometasksRepository;
        private readonly ILectureRepository _lecturesRepository;
        private readonly ILogger<AttendanceService> _logger;
        private readonly EmailProvider _emailProvider;
        private readonly SmsProvider _smsProvider;
        private readonly IMapper _mapper;

        public AttendanceService(IAttendanceRepository attendanceRepository, IHometaskRepository hometasksRepository,
            ILectureRepository lecturesRepository, EmailProvider emailProvider, SmsProvider smsProvider,
            ILogger<AttendanceService> logger, IMapper mapper)
        {
            _attendanceRepository = attendanceRepository;
            _hometasksRepository = hometasksRepository;
            _lecturesRepository = lecturesRepository;
            _emailProvider = emailProvider;
            _smsProvider = smsProvider;
            _logger = logger;
            _mapper = mapper;
        }

        public Attendance SetPresence(int lectureId, int studentId, DateTime lectureDate, bool presence, bool hometaskDone)
        {
            var attendanceDb = _attendanceRepository.Create(new AttendanceDb
            {
                LectureId = lectureId,
                StudentId = studentId,
                LectureDate = lectureDate,
                Presence = presence,
                HometaskDone = hometaskDone
            }
            );
            _logger.LogInformation($"The presence on the lecture {lectureId} of the student {studentId} for {lectureDate} was set.");

            if (attendanceDb.Presence == false || attendanceDb.HometaskDone == false)
            {
                var hometask = new HometaskDb()
                {
                    HometaskDate = DateTime.Now,
                    Mark = 0,
                    StudentId = studentId,
                    LectureId = lectureId
                };
                _hometasksRepository.Create(hometask);
                _logger.LogInformation("The hometask for the student with Id {} and the lecture with Id {} with the mark 0 was created.", studentId, lectureId);
                return _mapper.Map<Attendance>(attendanceDb);
            }
            var teacherId = _lecturesRepository.Get(attendanceDb.LectureId).TeacherId;
            _emailProvider.SendEmail(attendanceDb.LectureId, teacherId, attendanceDb.StudentId);
            _smsProvider.SendSms(attendanceDb.LectureId, attendanceDb.StudentId);
            return _mapper.Map<Attendance>(attendanceDb);
        }

        public Attendance GetWithLecture(int id)
        {
            var attendance = _attendanceRepository.GetWithLecture(id);
            if (attendance == null)
            {
                return null;
            }
            return _mapper.Map<Attendance>(attendance);
        }

        public Page<Attendance> GetByFilter(int? groupId, int? studentId, int? lectureId,
            DateTime? lectureDateFrom, DateTime? lectureDateTo, PageParams parameters)
        {
            var dbParams = _mapper.Map<DataAccess.PageParams>(parameters);
            var attendance = _attendanceRepository.GetByFilter(groupId, studentId, lectureId, lectureDateFrom, lectureDateTo, dbParams);
            if (attendance.Data == null)
            {
                return new Page<Attendance>();
            }
            return _mapper.Map<Page<Attendance>>(attendance);
        }

        public Page<Attendance> GetLectureAttendanceReport(int? lectureId, PageParams parameters)
        {
            var dbParams = _mapper.Map<DataAccess.PageParams>(parameters);
            var attendance = _attendanceRepository.GetByFilter(null, null, lectureId, null, null, dbParams);
            if (!attendance.Data.Any())
            {
                return new Page<Attendance>();
            }
            return _mapper.Map<Page<Attendance>>(attendance);
        }

        public Page<Attendance> GetStudentAttendanceReport(int? studentId, PageParams parameters)
        {
            var dbParams = _mapper.Map<DataAccess.PageParams>(parameters);
            var attendance = _attendanceRepository.GetByFilter(null, studentId, null, null, null, dbParams);
            if (!attendance.Data.Any())
            {
                return new Page<Attendance>();
            }
            return _mapper.Map<Page<Attendance>>(attendance);
        }

        public void Delete(int id)
        {
            EnsureAttendanceExist(id);
            _attendanceRepository.Delete(id);
        }

        private void EnsureAttendanceExist(int id)
        {
            var attendance = _attendanceRepository.Get(id);
            if (attendance == null)
            {
                throw new AttendanceNotFoundException($"There is no attendance with Id {id}.");
            }
        }
    }
}
