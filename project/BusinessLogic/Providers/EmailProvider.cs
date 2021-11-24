using DataAccess;
using Microsoft.Extensions.Logging;

namespace BusinessLogic
{
    public class EmailProvider : IEmailProvider
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IStudentsRepository _studentsRepository;
        private readonly ITeachersRepository _teachersRepository;
        private readonly ILecturesRepository _lecturesRepository;
        private readonly ILogger<EmailProvider> _logger;

        public EmailProvider(IAttendanceRepository attendanceRepository, ILecturesRepository lecturesRepository, 
            ITeachersRepository teachersRepository, IStudentsRepository studentsRepository, ILogger<EmailProvider> logger)
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