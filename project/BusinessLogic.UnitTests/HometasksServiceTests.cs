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
    public class HometasksServiceTests
    {
        private Mock<IMapper> _mapper;
        private HometasksService _hometasksService;
        private Mock<ILogger<HometasksService>> _logger;
        private Mock<IHometasksRepository> _hometasksRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            _hometasksRepository = new Mock<IHometasksRepository>();
            _hometasksRepository.Setup(s => s.Create(_testDbHometasks[0])).Returns(_testDbHometasks[0].Id);
            _hometasksRepository.Setup(s => s.Get(0)).Returns(_testDbHometasks[0]);
            _hometasksRepository.Setup(s => s.Get(1)).Returns(_testDbHometasks[1]);

            _mapper = new Mock<IMapper>();
            _mapper.Setup(s => s.Map<HometaskDb>(_testDtoHometasks[0])).Returns(_testDbHometasks[0]);
            _mapper.Setup(s => s.Map<HometaskDto>(_testDbHometasks[0])).Returns(_testDtoHometasks[0]);
            _mapper.Setup(s => s.Map<HometaskDto>(_testDbHometasks[1])).Returns(_testDtoHometasks[1]);

            _logger = new Mock<ILogger<HometasksService>>();

            _hometasksService = new HometasksService(_hometasksRepository.Object, _mapper.Object, _logger.Object);
        }

        [Test]
        public void Create_HometaskDto_ReturnsHometaskId()
        {
            var result = _hometasksService.Create(_testDtoHometasks[0]);
            Assert.AreEqual(_testDbHometasks[0].Id, result);
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Get_HometaskIsExists_ReturnsHometaskDto(int id)
        {
            var result = _hometasksService.Get(id);
            Assert.AreEqual(_testDtoHometasks[id], result);
        }

        [Test]
        public void Get_HometaskIsNotExists_ThrowsHometaskNotFoundException()
        {
            var nonExistingHometaskId = 3;
            Assert.Throws<HometaskNotFoundException>(() => _hometasksService.Get(nonExistingHometaskId));
        }

        [Test]
        public void GetAll_ListOfHometasksIsNotEmpty_ReturnsListOfHometasks()
        {
            _hometasksRepository.Setup(s => s.GetAll()).Returns(_testDbHometasks);
            _mapper.Setup(s => s.Map<IReadOnlyCollection<HometaskDto>>(_testDbHometasks)).Returns(_testDtoHometasks);
            var result = _hometasksService.GetAll();
            Assert.AreEqual(_testDtoHometasks, result);
        }

        [Test]
        public void GetAll_ListOfHometasksIsEmpty_ReturnsEmptyList()
        {
            _hometasksRepository.Setup(s => s.GetAll()).Returns(new List<HometaskDb>());
            _mapper.Setup(s => s.Map<List<HometaskDto>>(new List<HometaskDb>())).Returns(new List<HometaskDto>());
            var result = _hometasksService.GetAll();
            Assert.AreEqual(new List<HometaskDto>(), result);
        }

        [Test]
        public void Update_HometaskIsExists_ReturnsHometaskId()
        {
            var result = _hometasksService.Update(_testDtoHometasks[0].Id, _testDtoHometasks[0]);
            Assert.AreEqual(_testDbHometasks[0].Id, result);
            _hometasksRepository.Verify(s => s.Update(_testDbHometasks[0]));
        }

        [Test]
        public void Update_HometaskIsNotExists_ThrowsHometaskNotFoundException()
        {
            var nonExistingHometaskId = 3;
            var nonExistingHometask = new HometaskDto();
            Assert.Throws<HometaskNotFoundException>(() => _hometasksService.Update(nonExistingHometaskId, nonExistingHometask));
        }

        [TestCase(0)]
        [TestCase(1)]
        public void Delete_HometaskIsExists_ReturnsHometaskId(int id)
        {
            _hometasksService.Delete(_testDtoHometasks[id].Id);
            _hometasksRepository.Verify(s => s.Delete(_testDbHometasks[id].Id));
        }

        [Test]
        public void Delete_HometaskIsNotExists_ThrowsHometaskNotFoundException()
        {
            var nonExistingHometaskId = 3;
            Assert.Throws<HometaskNotFoundException>(() => _hometasksService.Delete(nonExistingHometaskId));
        }

        private readonly List<HometaskDto> _testDtoHometasks =
            new(new[]
            {
                new HometaskDto { Id = 0, HometaskDate = new DateTime(2013, 06, 12), Mark = 4, StudentId = 1, LectureId = 1 },
                new HometaskDto { Id = 1, HometaskDate = new DateTime(2013, 06, 12), Mark = 5, StudentId = 2, LectureId = 1 }
            });

        private readonly List<HometaskDb> _testDbHometasks =
            new(new[]
            {
                new HometaskDb { Id = 0, HometaskDate = new DateTime(2013, 06, 12), Mark = 4, StudentId = 1, LectureId = 1 },
                new HometaskDb { Id = 1, HometaskDate = new DateTime(2013, 06, 12), Mark = 5, StudentId = 2, LectureId = 1 }
            });
    }
}