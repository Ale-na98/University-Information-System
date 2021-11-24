using Moq;
using AutoMapper;
using DataAccess;
using NUnit.Framework;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.UnitTests
{
    [TestFixture]
    public class LecturesServiceTests
    {
        private Mock<IMapper> _mapper;
        private LecturesService _lecturesService;
        private Mock<ILogger<LecturesService>> _logger;
        private Mock<ILecturesRepository> _lecturesRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            _lecturesRepository = new Mock<ILecturesRepository>();
            _lecturesRepository.Setup(s => s.Create(_testDbLectures[0])).Returns(_testDbLectures[0].Id);
            _lecturesRepository.Setup(s => s.Get(0)).Returns(_testDbLectures[0]);
            _lecturesRepository.Setup(s => s.Get(1)).Returns(_testDbLectures[1]);

            _mapper = new Mock<IMapper>();
            _mapper.Setup(s => s.Map<LectureDb>(_testDtoLectures[0])).Returns(_testDbLectures[0]);
            _mapper.Setup(s => s.Map<LectureDto>(_testDbLectures[0])).Returns(_testDtoLectures[0]);
            _mapper.Setup(s => s.Map<LectureDto>(_testDbLectures[1])).Returns(_testDtoLectures[1]);

            _logger = new Mock<ILogger<LecturesService>>();

            _lecturesService = new LecturesService(_lecturesRepository.Object, _mapper.Object, _logger.Object);
        }

        [Test]
        public void Create_LectureDto_ReturnsLectureId()
        {
            var result = _lecturesService.Create(_testDbLectures[0].Name);
            Assert.AreEqual(_testDbLectures[0].Id, result);
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Get_LectureIsExists_ReturnsLectureDto(int id)
        {
            var result = _lecturesService.Get(id);
            Assert.AreEqual(_testDtoLectures[id], result);
        }

        [Test]
        public void Get_LectureIsNotExists_ThrowsLectureNotFoundException()
        {
            var nonExistingStudentId = 3;
            Assert.Throws<LectureNotFoundException>(() => _lecturesService.Get(nonExistingStudentId));
        }

        [Test]
        public void GetAll_ListOfLecturesIsNotEmpty_ReturnsListOfLectures()
        {
            _lecturesRepository.Setup(s => s.GetAll()).Returns(_testDbLectures);
            _mapper.Setup(s => s.Map<IReadOnlyCollection<LectureDto>>(_testDbLectures)).Returns(_testDtoLectures);
            var result = _lecturesService.GetAll();
            Assert.AreEqual(_testDtoLectures, result);
        }

        [Test]
        public void GetAll_ListOfLecturesIsEmpty_ReturnsEmptyList()
        {
            _lecturesRepository.Setup(s => s.GetAll()).Returns(new List<LectureDb>());
            _mapper.Setup(s => s.Map<List<LectureDto>>(new List<LectureDb>())).Returns(new List<LectureDto>());
            var result = _lecturesService.GetAll();
            Assert.AreEqual(new List<LectureDto>(), result);
        }

        [Test]
        public void Update_LectureIsExists_ReturnsLectureId()
        {
            var result = _lecturesService.Update(_testDtoLectures[0].Id, _testDtoLectures[0]);
            Assert.AreEqual(_testDbLectures[0].Id, result);
            _lecturesRepository.Verify(s => s.Update(_testDbLectures[0]));
        }

        [Test]
        public void Update_LectureIsNotExists_ThrowsLectureNotFoundException()
        {
            var nonExistingLectureId = 3;
            var nonExistingLecture = new LectureDto();
            Assert.Throws<LectureNotFoundException>(() => _lecturesService.Update(nonExistingLectureId, nonExistingLecture));
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Delete_LectureIsExists_ReturnsLectureId(int id)
        {
            _lecturesService.Delete(_testDbLectures[id].Id);
            _lecturesRepository.Verify(s => s.Delete(_testDbLectures[id].Id));
        }

        [Test]
        public void Delete_LectureIsNotExists_ThrowsLectureNotFoundException()
        {
            var nonLectureStudentId = 3;
            Assert.Throws<LectureNotFoundException>(() => _lecturesService.Delete(nonLectureStudentId));
        }


        private readonly List<LectureDto> _testDtoLectures =
            new(new[]
            {
                new LectureDto { Id = 0, Name = "Maths", TeacherId = null },
                new LectureDto { Id = 1, Name = "Physics", TeacherId = 1 }
            });

        private readonly List<LectureDb> _testDbLectures =
            new(new[]
            {
                new LectureDb { Id = 0, Name = "Maths", TeacherId = null },
                new LectureDb { Id = 1, Name = "Physics", TeacherId = 1 }
            });
    }
}