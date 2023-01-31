using System;
using DataAccess.Interfaces;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Providers
{
    public class SmsProvider
    {
        private readonly ILectureRepository _lecturesRepository;
        private readonly IHometaskRepository _hometasksRepository;
        private readonly IStudentRepository _studentsRepository;
        private readonly ILogger<SmsProvider> _logger;

        public SmsProvider(IHometaskRepository hometasksRepository, ILectureRepository lecturesRepository, IStudentRepository studentsRepository, ILogger<SmsProvider> logger)
        {
            _hometasksRepository = hometasksRepository;
            _studentsRepository = studentsRepository;
            _lecturesRepository = lecturesRepository;
            _logger = logger;
        }
        public string SendSms(int lectureId, int studentId)
        {
            var averageMark = _hometasksRepository.GetAverageMark(lectureId, studentId);
            if (averageMark < 4)
            {
                var lecture = _lecturesRepository.Get(lectureId);
                var student = _studentsRepository.Get(studentId);
                var message = $"The SMS sent to the student {student.FullName} to {student.PhoneNumber}, because his/her average mark in {lecture.Name} is {String.Format("{0:0.##}", averageMark)}.";
                _logger.LogInformation(message);
                return message;
            }
            return string.Empty;
        }
    }
}
