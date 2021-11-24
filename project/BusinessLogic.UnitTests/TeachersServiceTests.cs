using Moq;
using AutoMapper;
using DataAccess;
using NUnit.Framework;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.UnitTests
{
    [TestFixture]
    public class TeachersServiceTests
    {
        private Mock<IMapper> _mapper;
        private TeachersService _teachersService;
        private Mock<ILogger<TeachersService>> _logger;
        private Mock<ITeachersRepository> _teachersRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            _teachersRepository = new Mock<ITeachersRepository>();
            _teachersRepository.Setup(s => s.Create(_testDbTeachers[0])).Returns(_testDbTeachers[0].Id);
            _teachersRepository.Setup(s => s.Get(0)).Returns(_testDbTeachers[0]);
            _teachersRepository.Setup(s => s.Get(1)).Returns(_testDbTeachers[1]);

            _mapper = new Mock<IMapper>();
            _mapper.Setup(s => s.Map<TeacherDb>(_testDtoTeachers[0])).Returns(_testDbTeachers[0]);
            _mapper.Setup(s => s.Map<TeacherDto>(_testDbTeachers[0])).Returns(_testDtoTeachers[0]);
            _mapper.Setup(s => s.Map<TeacherDto>(_testDbTeachers[1])).Returns(_testDtoTeachers[1]);

            _logger = new Mock<ILogger<TeachersService>>();

            _teachersService = new TeachersService(_teachersRepository.Object, _mapper.Object, _logger.Object);
        }

        [Test]
        public void Create_TeacherDto_ReturnsTeacherId()
        {
            var result = _teachersService.Create(_testDtoTeachers[0]);
            Assert.AreEqual(_testDbTeachers[0].Id, result);
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Get_TeacherIsExists_ReturnsTeacherDto(int id)
        {
            var result = _teachersService.Get(id);
            Assert.AreEqual(_testDtoTeachers[id], result);
        }

        [Test]
        public void Get_TeacherIsNotExists_ThrowsTeacherNotFoundException()
        {
            var nonExistingTeacherId = 3;
            Assert.Throws<TeacherNotFoundException>(() => _teachersService.Get(nonExistingTeacherId));
        }

        [Test]
        public void GetAll_ListOfTeachersIsNotEmpty_ReturnsListOfTeachers()
        {
            _teachersRepository.Setup(s => s.GetAll()).Returns(_testDbTeachers);
            _mapper.Setup(s => s.Map<IReadOnlyCollection<TeacherDto>>(_testDbTeachers)).Returns(_testDtoTeachers);
            var result = _teachersService.GetAll();
            Assert.AreEqual(_testDtoTeachers, result);
        }

        [Test]
        public void GetAll_ListOfTeachersIsEmpty_ReturnsEmptyList()
        {
            _teachersRepository.Setup(s => s.GetAll()).Returns(new List<TeacherDb>());
            _mapper.Setup(s => s.Map<List<TeacherDto>>(new List<TeacherDb>())).Returns(new List<TeacherDto>());
            var result = _teachersService.GetAll();
            Assert.AreEqual(new List<TeacherDto>(), result);
        }

        [Test]
        public void Update_TeacherIsExists_ReturnsTeacherId()
        {
            var result = _teachersService.Update(_testDtoTeachers[0].Id, _testDtoTeachers[0]);
            Assert.AreEqual(_testDbTeachers[0].Id, result);
            _teachersRepository.Verify(s => s.Update(_testDbTeachers[0]));
        }

        [Test]
        public void Update_TeacherIsNotExists_ThrowsTeacherNotFoundException()
        {
            var nonExistingTeacherId = 3;
            var nonExistingTeacher = new TeacherDto();
            Assert.Throws<TeacherNotFoundException>(() => _teachersService.Update(nonExistingTeacherId, nonExistingTeacher));
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Delete_TeacherIsExists_ReturnsTeacherId(int id)
        {
            _teachersService.Delete(_testDtoTeachers[id].Id);
            _teachersRepository.Verify(s => s.Delete(_testDbTeachers[id].Id));
        }

        [Test]
        public void Delete_TeacherIsNotExists_ThrowsTeacherNotFoundException()
        {
            var nonExistingTeacherId = 3;
            Assert.Throws<TeacherNotFoundException>(() => _teachersService.Delete(nonExistingTeacherId));
        }


        private readonly List<TeacherDto> _testDtoTeachers =
            new(new[]
            {
                new TeacherDto { Id = 0, FullName = "Roman Kozlov", Email = "romankozlov@gmail.com" },
                new TeacherDto { Id = 1, FullName = "Maxim Zakharov", Email = "maximzakharov@gmail.com" }
            });

        private readonly List<TeacherDb> _testDbTeachers =
            new(new[]
            {
                new TeacherDb { Id = 0, FullName = "Roman Kozlov", Email = "romankozlov@gmail.com" },
                new TeacherDb { Id = 1, FullName = "Maxim Zakharov", Email = "maximzakharov@gmail.com" }
            });
    }
}