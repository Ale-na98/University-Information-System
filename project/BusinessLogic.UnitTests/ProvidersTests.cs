using Moq;
using System;
using AutoMapper;
using DataAccess;
using NUnit.Framework;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.UnitTests
{
    [TestFixture]
    public class ProvidersTests
    {
        private StudentDb _student;
        private TeacherDb _teacher;
        private LectureDb _lecture;
        private SmsProvider _smsProvider;
        private EmailProvider _emailProvider;
        private Mock<ILogger<SmsProvider>> _loggerSmsProvider;
        private Mock<IStudentsRepository> _studentsRepository;
        private Mock<ITeachersRepository> _teachersRepository;
        private Mock<ILecturesRepository> _lecturesRepository;
        private Mock<IHometasksRepository> _hometasksRepository;
        private Mock<IAttendanceRepository> _attendanceRepository;
        private Mock<ILogger<EmailProvider>> _loggerEmailProvider;

        [OneTimeSetUp]
        public void SetUp()
        {
            _attendanceRepository = new Mock<IAttendanceRepository>();

            _studentsRepository = new Mock<IStudentsRepository>();
            _student = new StudentDb() { Id = 0, FullName = "Roman Kozlov", Email = "romankozlov@gmail.com", PhoneNumber = "89169475896" };
            _studentsRepository.Setup(s => s.Get(0)).Returns(_student);

            _teachersRepository = new Mock<ITeachersRepository>();
            _teacher = new TeacherDb() { Id = 0, FullName = "Ivan Ivanov", Email = "ivanivanov@gmail.com" };
            _teachersRepository.Setup(s => s.Get(0)).Returns(_teacher);

            _hometasksRepository = new Mock<IHometasksRepository>();
            _loggerEmailProvider = new Mock<ILogger<EmailProvider>>();
            _loggerSmsProvider = new Mock<ILogger<SmsProvider>>();

            _lecturesRepository = new Mock<ILecturesRepository>();
            _lecture = new LectureDb() { Id = 0, Name = "Maths" };
            _lecturesRepository.Setup(s => s.Get(0)).Returns(_lecture);

            _emailProvider = new EmailProvider(_attendanceRepository.Object, _lecturesRepository.Object, _teachersRepository.Object, _studentsRepository.Object, _loggerEmailProvider.Object);
            _smsProvider = new SmsProvider(_hometasksRepository.Object, _lecturesRepository.Object, _studentsRepository.Object, _loggerSmsProvider.Object);
        }

        [Test]
        public void SendEmail_NumberOfAbsenteeismLessThan3_ReturnsEmptyString()
        {
            _attendanceRepository.Setup(s => s.GetNumberOfAbsenteeism(It.IsAny<int>(), It.IsAny<int>())).Returns(2);
            var result = _emailProvider.SendEmail(0, 0, 0);
            Assert.AreEqual(string.Empty, result);
        }

        [TestCase(3)]
        [TestCase(5)]
        public void SendEmail_NumberOfAbsenteeismEqualOrMoreThan3_ReturnsMessage(int numberOfAbsenteeism)
        {
            _attendanceRepository.Setup(s => s.GetNumberOfAbsenteeism(It.IsAny<int>(), It.IsAny<int>())).Returns(numberOfAbsenteeism);
            var expected = $"The Email sent to the teacher {_teacher.FullName} and the student {_student.FullName} to {_student.Email}, because the student miss more than 3 {_lecture.Name} lectures.";
            var result = _emailProvider.SendEmail(0, 0, 0);
            Assert.AreEqual(expected, result);
        }

        [TestCase(4)]
        [TestCase(4.5)]
        public void SendSms_AverageMarkMoreThan4_ReturnsEmptyString(double averageMark)
        {
            _hometasksRepository.Setup(s => s.GetAverageMark(It.IsAny<int>(), It.IsAny<int>())).Returns(averageMark);
            var result = _smsProvider.SendSms(0, 0);
            Assert.AreEqual(string.Empty, result);
        }
        
        [Test]
        public void SendSms_AverageMarkEqualOrLessThan4_ReturnsMessage()
        {
            _hometasksRepository.Setup(s => s.GetAverageMark(It.IsAny<int>(), It.IsAny<int>())).Returns(3);
            var expected = $"The SMS sent to the student {_student.FullName} to {_student.PhoneNumber}, because his/her average mark in {_lecture.Name} is 3.";
            var result = _smsProvider.SendSms(0, 0);
            Assert.AreEqual(expected, result);
        }
    }
}