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
    public class AttendanceServiceTests
    {       
        private Mock<IMapper> _mapper;
        private Mock<ISmsProvider> _smsProvider;
        private Mock<IEmailProvider> _emailProvider;
        private AttendanceService _attendanceService;
        private Mock<ILogger<AttendanceService>> _logger;
        private Mock<ILecturesRepository> _lecturesRepository;
        private Mock<IHometasksRepository> _hometasksRepository;
        private Mock<IAttendanceRepository> _attendanceRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            _attendanceRepository = new Mock<IAttendanceRepository>();
            _hometasksRepository = new Mock<IHometasksRepository>();
            _logger = new Mock<ILogger<AttendanceService>>();
            _mapper = new Mock<IMapper>();

            _lecturesRepository = new Mock<ILecturesRepository>();
            _lecturesRepository.Setup(s => s.Get(0)).Returns(_testDbLectures[0]);
            _lecturesRepository.Setup(s => s.Get(1)).Returns(_testDbLectures[1]);

            _emailProvider = new Mock<IEmailProvider>();
            _emailProvider.Setup(s => s.SendEmail(_testDbLectures[0].Id, _testDbTeachers[0].Id, _testDbStudents[0].Id)).Returns(string.Empty);

            _smsProvider = new Mock<ISmsProvider>();
            _smsProvider.Setup(s => s.SendSms(_testDbLectures[0].Id, _testDbStudents[0].Id)).Returns(string.Empty);

            _attendanceService = new AttendanceService(_attendanceRepository.Object, _hometasksRepository.Object, _lecturesRepository.Object,
                _emailProvider.Object, _smsProvider.Object, _logger.Object, _mapper.Object);
        }

        [Test]
        public void SetPresence_CreateAttendanceRecord_VerifyAttendanceCreateMethod()
        {
            _attendanceRepository.Setup(s => s.Create(_testDbAttendance[0]));
            _mapper.Setup(m => m.Map<AttendanceDb>(_testDtoAttendance[0])).Returns(_testDbAttendance[0]);
            _attendanceService.SetPresence(_testDtoAttendance[0]);
            _attendanceRepository.Verify(s => s.Create(_testDbAttendance[0]));
        }

        [Test]
        public void SetPresence_CreateAttendanceRecordWithHometaskDoneFalse_VerifyHometaskCreateMethodWithMark0()
        {
            HometaskDb result = null;
            _hometasksRepository.Setup(s => s.Create(It.IsAny<HometaskDb>())).Callback<HometaskDb>(r => result = r);
            _mapper.Setup(m => m.Map<AttendanceDb>(_testDtoAttendance[1])).Returns(_testDbAttendance[1]);
            _attendanceService.SetPresence(_testDtoAttendance[1]);
            Assert.AreEqual(0, result.Mark);
        }

        [Test]
        public void SetPresence_CreateAttendanceRecordWithPresenceFalse_VerifyHometaskCreateMethodWithMark0()
        {
            HometaskDb result = null;
            _hometasksRepository.Setup(s => s.Create(It.IsAny<HometaskDb>())).Callback<HometaskDb>(r => result = r);
            _mapper.Setup(m => m.Map<AttendanceDb>(_testDtoAttendance[2])).Returns(_testDbAttendance[2]);
            _attendanceService.SetPresence(_testDtoAttendance[2]);
            Assert.AreEqual(0, result.Mark);
        }

        [TestCase(0, 0, 0)]
        [TestCase(2, 1, 1)]
        public void Get_AttendanceIsExists_ReturnsAttendanceDto(int id, int lectureId, int studentId)
        {
            _attendanceRepository.Setup(s => s.Get(_testDbAttendance[id].LectureId, _testDbAttendance[id].StudentId, _testDbAttendance[id].LectureDate))
                .Returns(_testDbAttendance[id]);
            _mapper.Setup(m => m.Map<AttendanceDto>(_testDbAttendance[0])).Returns(_testDtoAttendance[0]);
            _mapper.Setup(m => m.Map<AttendanceDto>(_testDbAttendance[2])).Returns(_testDtoAttendance[2]);
            var result = _attendanceService.Get(lectureId, studentId, new DateTime(2013, 06, 12));
            Assert.AreEqual(_testDtoAttendance[id], result);
        }

        [Test]
        public void Get_AttendanceIsNotExists_ThrowsAttendanceNotFoundException()
        {
            var nonExistingLectureId = 5;
            var nonExistingStudentId = 5;
            Assert.Throws<AttendanceNotFoundException>(() => _attendanceService.Get(nonExistingLectureId, nonExistingStudentId, new DateTime(2013, 06, 14)));
        }

        [Test]
        public void CreateLectureAttendanceReport_LectureAttendanceReportIsNotEmpty_ReturnsListOfLectureAttendance()
        {
            //Arrange
            List<AttendanceReportDto> romanKozlovAttendanceReport = new()
            {
                new AttendanceReportDto { LectureDate = new DateTime(2013, 06, 12), Presence = true, HometaskDone = true },
                new AttendanceReportDto { LectureDate = new DateTime(2013, 06, 13), Presence = true, HometaskDone = false }
            };
            List<AttendanceReportDto> maximZakharovAttendanceReport = new()
            {
                new AttendanceReportDto { LectureDate = new DateTime(2013, 06, 14), Presence = true, HometaskDone = false }
            };
            List<StudentAttendanceDto> lectureAttendanceReport = new()
            {
                new StudentAttendanceDto { FullName = _testDbStudents[0].FullName, Attendance = romanKozlovAttendanceReport },
                new StudentAttendanceDto { FullName = _testDbStudents[1].FullName, Attendance = maximZakharovAttendanceReport },
            };

            var mathsAttendance = _testDbAttendance.FindAll(a => a.Lecture.Name == "Maths");
            var romanKozlovMathsAttendance = mathsAttendance.FindAll(a => a.Student.FullName == "Roman Kozlov");
            var maximZakharovMathsAttendance = mathsAttendance.FindAll(a => a.Student.FullName == "Maxim Zakharov");

            _attendanceRepository.Setup(s => s.GetLectureAttendance("Maths")).Returns(mathsAttendance);
            _mapper.Setup(m => m.Map<List<AttendanceReportDto>>(romanKozlovMathsAttendance)).Returns(romanKozlovAttendanceReport);
            _mapper.Setup(m => m.Map<List<AttendanceReportDto>>(maximZakharovMathsAttendance)).Returns(maximZakharovAttendanceReport);

            //Act
            var result = _attendanceService.CreateLectureAttendanceReport("Maths");

            //Assert
            Assert.AreEqual(lectureAttendanceReport, result);
        }

        [Test]
        public void CreateLectureAttendanceReport_LectureAttendanceReportIsEmpty_ReturnsEmptyList()
        {
            _attendanceRepository.Setup(s => s.GetLectureAttendance("Physics")).Returns(new List<AttendanceDb>());
            var result = _attendanceService.CreateLectureAttendanceReport("Physics");
            Assert.AreEqual(new List<StudentAttendanceDto>(), result);
        }

        [Test]
        public void CreateStudentAttendanceReport_StudentAttendanceReportIsNotEmpty_ReturnsListOfStudentAttendance()
        {
            //Arrange
            List<AttendanceReportDto> lectureAttendanceReport = new()
            {
                new AttendanceReportDto { LectureDate = new DateTime(2013, 06, 12), Presence = true, HometaskDone = true },
                new AttendanceReportDto { LectureDate = new DateTime(2013, 06, 13), Presence = true, HometaskDone = false }
            };
            List<LectureAttendanceDto> studentAttendanceReport = new()
            {
                new LectureAttendanceDto { Name = _testDbLectures[0].Name, Attendance = lectureAttendanceReport }
            };

            var romanKozlovAttendance = _testDbAttendance.FindAll(a => a.Student.FullName == "Roman Kozlov");

            _attendanceRepository.Setup(s => s.GetStudentAttendance("Roman Kozlov")).Returns(romanKozlovAttendance);
            _mapper.Setup(m => m.Map<List<AttendanceReportDto>>(romanKozlovAttendance)).Returns(lectureAttendanceReport);

            //Act
            var result = _attendanceService.CreateStudentAttendanceReport("Roman Kozlov");

            //Assert
            Assert.AreEqual(studentAttendanceReport, result);
        }

        [Test]
        public void CreateStudentAttendanceReport_StudentAttendanceReportIsEmpty_ReturnsEmptyList()
        {
            _attendanceRepository.Setup(s => s.GetStudentAttendance("Maxim Zakharov")).Returns(new List<AttendanceDb>());
            var result = _attendanceService.CreateStudentAttendanceReport("Maxim Zakharov");
            Assert.AreEqual(new List<LectureAttendanceDto>(), result);
        }

        private readonly List<AttendanceDb> _testDbAttendance =
            new(new[]
            {
                new AttendanceDb { LectureId = 0, Lecture = _testDbLectures[0], StudentId = 0, Student = _testDbStudents[0], LectureDate = new DateTime(2013, 06, 12), Presence = true, HometaskDone = true },
                new AttendanceDb { LectureId = 0, Lecture = _testDbLectures[0], StudentId = 0, Student = _testDbStudents[0], LectureDate = new DateTime(2013, 06, 13), Presence = true, HometaskDone = false },
                new AttendanceDb { LectureId = 1, Lecture = _testDbLectures[1], StudentId = 1, Student = _testDbStudents[1], LectureDate = new DateTime(2013, 06, 12), Presence = false, HometaskDone = false },
                new AttendanceDb { LectureId = 0, Lecture = _testDbLectures[0], StudentId = 1, Student = _testDbStudents[1], LectureDate = new DateTime(2013, 06, 14), Presence = true, HometaskDone = false }
            });

        private readonly List<AttendanceDto> _testDtoAttendance =
            new(new[]
            {
                new AttendanceDto { LectureId = 0, StudentId = 0, LectureDate = new DateTime(2013, 06, 12), Presence = true, HometaskDone = true },
                new AttendanceDto { LectureId = 0, StudentId = 0, LectureDate = new DateTime(2013, 06, 13), Presence = true, HometaskDone = false },
                new AttendanceDto { LectureId = 1, StudentId = 1, LectureDate = new DateTime(2013, 06, 12), Presence = false, HometaskDone = false },
                new AttendanceDto { LectureId = 0, StudentId = 1, LectureDate = new DateTime(2013, 06, 14), Presence = true, HometaskDone = false }
            });

        private static readonly List<StudentDb> _testDbStudents =
            new(new[]
            {
                new StudentDb { Id = 0, FullName = "Roman Kozlov", Email = "romankozlov@gmail.com", PhoneNumber = "89169475896" },
                new StudentDb { Id = 1, FullName = "Maxim Zakharov", Email = "maximzakharov@gmail.com", PhoneNumber = "89129475994" }
            });

        private readonly List<TeacherDb> _testDbTeachers =
            new(new[]
            {
                new TeacherDb { Id = 0, FullName = "Ivan Ivanov", Email = "ivanivanov@gmail.com" },
                new TeacherDb { Id = 2, FullName = "Peter Petrov", Email = "peterpetrov@gmail.com" }
            });

        private static readonly List<LectureDb> _testDbLectures =
            new(new[]
            {
                new LectureDb { Id = 0, Name = "Maths", TeacherId = 0 },
                new LectureDb { Id = 1, Name = "Physics", TeacherId = 1 }
            });
    }
}
