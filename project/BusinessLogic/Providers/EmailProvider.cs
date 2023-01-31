using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Providers
{
    public class EmailProvider
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IStudentRepository _studentsRepository;
        private readonly IUniversityRepository<TeacherDb> _teachersRepository;
        private readonly ILectureRepository _lecturesRepository;
        private readonly ILogger<EmailProvider> _logger;

        public EmailProvider(IAttendanceRepository attendanceRepository, ILectureRepository lecturesRepository,
            IUniversityRepository<TeacherDb> teachersRepository, IStudentRepository studentsRepository, ILogger<EmailProvider> logger)
        {
            _attendanceRepository = attendanceRepository;
            _studentsRepository = studentsRepository;
            _teachersRepository = teachersRepository;
            _lecturesRepository = lecturesRepository;
            _logger = logger;
        }

        public string SendEmail(int lectureId, int? teacherId, int studentId)
        {
            var numberOfAbsenteeism = _attendanceRepository.GetNumberOfAbsenteeism(lectureId, studentId);
            if (numberOfAbsenteeism >= 3)
            {
                var lecture = _lecturesRepository.Get(lectureId);
                var teacher = _teachersRepository.Get(teacherId.GetValueOrDefault());
                var student = _studentsRepository.Get(studentId);
                var message = $"The Email sent to the teacher {teacher.FullName} and the student {student.FullName} to {student.Email}, because the student miss more than 3 {lecture.Name} lectures.";
                _logger.LogInformation(message);
                return message;
            }
            return string.Empty;
        }
    }
}
