using Moq;
using AutoMapper;
using DataAccess;
using NUnit.Framework;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.UnitTests
{
    [TestFixture]
    public class StudentsServiceTests
    {
        private Mock<IMapper> _mapper; 
        private StudentsService _studentsService;
        private Mock<ILogger<StudentsService>> _logger;
        private Mock<IStudentsRepository> _studentsRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            _studentsRepository = new Mock<IStudentsRepository>();
            _studentsRepository.Setup(s => s.Create(_testDbStudents[0])).Returns(_testDbStudents[0].Id);
            _studentsRepository.Setup(s => s.Get(0)).Returns(_testDbStudents[0]);
            _studentsRepository.Setup(s => s.Get(1)).Returns(_testDbStudents[1]);

            _mapper = new Mock<IMapper>();
            _mapper.Setup(s => s.Map<StudentDb>(_testDtoStudents[0])).Returns(_testDbStudents[0]);
            _mapper.Setup(s => s.Map<StudentDto>(_testDbStudents[0])).Returns(_testDtoStudents[0]);
            _mapper.Setup(s => s.Map<StudentDto>(_testDbStudents[1])).Returns(_testDtoStudents[1]);

            _logger = new Mock<ILogger<StudentsService>>();

            _studentsService = new StudentsService(_studentsRepository.Object, _mapper.Object, _logger.Object);
        }

        [Test]
        public void Create_StudentDto_ReturnsStudentId()
        {
            var result = _studentsService.Create(_testDtoStudents[0]);
            Assert.AreEqual(_testDbStudents[0].Id, result);
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Get_StudentIsExists_ReturnsStudentDto(int id)
        {
            var result = _studentsService.Get(id);
            Assert.AreEqual(_testDtoStudents[id], result);
        }

        [Test]
        public void Get_StudentIsNotExists_ThrowsStudentNotFoundException()
        {
            var nonExistingStudentId = 3;
            Assert.Throws<StudentNotFoundException>(() => _studentsService.Get(nonExistingStudentId));
        }

        [Test]
        public void GetAll_ListOfStudentsIsNotEmpty_ReturnsListOfStudents()
        {
            _studentsRepository.Setup(s => s.GetAll()).Returns(_testDbStudents);
            _mapper.Setup(s => s.Map<IReadOnlyCollection<StudentDto>>(_testDbStudents)).Returns(_testDtoStudents);
            var result = _studentsService.GetAll();
            Assert.AreEqual(_testDtoStudents, result);
        }

        [Test]
        public void GetAll_ListOfStudentsIsEmpty_ReturnsEmptyList()
        {
            _studentsRepository.Setup(s => s.GetAll()).Returns(new List<StudentDb>());
            _mapper.Setup(s => s.Map<List<StudentDto>>(new List<StudentDb>())).Returns(new List<StudentDto>());
            var result = _studentsService.GetAll();
            Assert.AreEqual(new List<StudentDto>(), result);
        }

        [Test]
        public void Update_StudentIsExists_ReturnsStudentId()
        {
            var result = _studentsService.Update(_testDtoStudents[0].Id, _testDtoStudents[0]);
            Assert.AreEqual(_testDbStudents[0].Id, result);
            _studentsRepository.Verify(s => s.Update(_testDbStudents[0]));
        }

        [Test]
        public void Update_StudentIsNotExists_ThrowsStudentNotFoundException()
        {
            var nonExistingStudentId = 3;
            var nonExistingStudent = new StudentDto();
            Assert.Throws<StudentNotFoundException>(() => _studentsService.Update(nonExistingStudentId, nonExistingStudent));
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Delete_StudentIsExists_ReturnsStudentId(int id)
        {
            _studentsService.Delete(_testDtoStudents[id].Id);
            _studentsRepository.Verify(s => s.Delete(_testDbStudents[id].Id));
        }

        [Test]
        public void Delete_StudentIsNotExists_ThrowsStudentNotFoundException()
        {
            var nonExistingStudentId = 3;
            Assert.Throws<StudentNotFoundException>(() => _studentsService.Delete(nonExistingStudentId));
        }


        private readonly List<StudentDto> _testDtoStudents =
            new(new[]
            {
                new StudentDto { Id = 0, FullName = "Roman Kozlov", Email = "romankozlov@gmail.com", PhoneNumber = "89169475896" },
                new StudentDto { Id = 1, FullName = "Maxim Zakharov", Email = "maximzakharov@gmail.com", PhoneNumber = "89129475994" }
            });

        private readonly List<StudentDb> _testDbStudents =
            new(new[]
            {
                new StudentDb { Id = 0, FullName = "Roman Kozlov", Email = "romankozlov@gmail.com", PhoneNumber = "89169475896" },
                new StudentDb { Id = 1, FullName = "Maxim Zakharov", Email = "maximzakharov@gmail.com", PhoneNumber = "89129475994" }
            });
    }
}