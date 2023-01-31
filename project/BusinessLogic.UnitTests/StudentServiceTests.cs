using Moq;
using Nest;
using AutoMapper;
using NUnit.Framework;
using DataAccess.Entities;
using BusinessLogic.Domain;
using DataAccess.Interfaces;
using BusinessLogic.Services;
using BusinessLogic.Exceptions;
using DataAccess.Elasticsearch;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using DataAccess.Elasticsearch.Documents;

namespace BusinessLogic.UnitTests
{
    [TestFixture]
    public class StudentServiceTests
    {
        private Mock<IMapper> _mapper;
        private StudentService _studentsService;
        private Mock<IElasticClient> _elasticClient;
        private Mock<ILogger<StudentService>> _logger;
        private Mock<IStudentRepository> _studentRepository;
        private Mock<IUniversityRepository<GroupDb>> _groupRepository;
        private Mock<ElasticsearchRepository<StudentDocument>> _elasticsearchRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            _elasticClient = new Mock<IElasticClient>();
            _studentRepository = new Mock<IStudentRepository>();
            _groupRepository = new Mock<IUniversityRepository<GroupDb>>();
            _elasticsearchRepository = new Mock<ElasticsearchRepository<StudentDocument>>(_elasticClient.Object);

            _mapper = new Mock<IMapper>();
            _mapper.Setup(s => s.Map<Student>(_dbStudents[0])).Returns(_students[0]);
            _mapper.Setup(s => s.Map<DataAccess.PageParams>(_blPageParams)).Returns(_daPageParams);

            _logger = new Mock<ILogger<StudentService>>();

            _studentsService = new StudentService(_studentRepository.Object, _groupRepository.Object,
                _elasticsearchRepository.Object, _mapper.Object, _logger.Object);
        }

        [Test]
        public void Create_ShouldCreateStudent()
        {
            //Arrange
            var student = _dbStudents[0] with { Id = 0 };
            _studentRepository.Setup(s => s.Create(student)).Returns(_dbStudents[0]);
            //_elasticsearchRepository.Setup(s => s.SaveSingle(It.IsAny<StudentDb>())).Verifiable();

            //Act
            var actual = _studentsService.Create(_students[0]);

            //Assert
            Assert.AreEqual(_students[0], actual);
        }

        [TestCaseSource(nameof(_getStudentCases))]
        public void GetWithGroup_ShouldProvideStudentById(int id, Student expected)
        {
            //Arrange
            _studentRepository.Setup(s => s.GetWithGroup(1)).Returns(_dbStudents[0]);

            //Act
            var actual = _studentsService.GetWithGroup(id);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCaseSource(nameof(_getAllStudentsCases))]
        public void GetAllWithGroups_ShouldProvideAllStudentsWithGroups(DataAccess.Page<StudentDb> pageCase, Page<Student> expected)
        {
            //Arrange
            _studentRepository.Setup(s => s.GetAllWithGroups(_daPageParams)).Returns(pageCase);
            _mapper.Setup(s => s.Map<Page<Student>>(pageCase)).Returns(expected);

            //Act
            var actual = _studentsService.GetAllWithGroups(_blPageParams);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCaseSource(nameof(_getAllStudentsByGroupIdCases))]
        public void GetAllByGroupId_ShouldProvideAllStudentsByGroupId(List<StudentDb> students, int? groupId, List<Student> expected)
        {
            //Arrange
            _studentRepository.Setup(s => s.GetAllByGroupId(groupId)).Returns(students);
            _mapper.Setup(s => s.Map<IList<Student>>(students)).Returns(expected);

            //Act
            var actual = _studentsService.GetAllByGroupId(groupId);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCaseSource(nameof(_getAllStudentsByFilterCases))]
        public void GetByFilter_ShouldProvideAllStudentsByByFilter(int? id, string fullName, string email, string phoneNumber,
            string group, DataAccess.Page<StudentDb> pageCase, Page<Student> expected)
        {
            //Arrange
            _studentRepository.Setup(s => s.GetByFilter(id, fullName, email, phoneNumber, group, _daPageParams)).Returns(pageCase);
            _mapper.Setup(s => s.Map<Page<Student>>(pageCase)).Returns(expected);

            //Act
            var actual = _studentsService.GetByFilter(id, fullName, email, phoneNumber, group, _blPageParams);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Update_PhoneNumberIsChanged_ShouldUpdateStudent()
        {
            //Arrange
            var updatedStudent = new StudentDb()
            {
                Id = 1,
                FullName = "Roman Kozlov",
                Email = "romankozlov@gmail.com",
                PhoneNumber = "379652",
                Group = new GroupDb { Id = 1 }
            };

            var expected = new Student()
            {
                Id = 1,
                FullName = "Roman Kozlov",
                Email = "romankozlov@gmail.com",
                PhoneNumber = "379652",
                Group = new Group { Id = 1 }
            };

            _studentRepository.Setup(s => s.Get(updatedStudent.Id)).Returns(updatedStudent);
            _studentRepository.Setup(s => s.Update(updatedStudent)).Returns(updatedStudent);
            _groupRepository.Setup(s => s.Get(updatedStudent.Group.Id)).Returns(new GroupDb() { Id = 1, Name = "6501" });
            _mapper.Setup(s => s.Map<StudentDb>(expected)).Returns(updatedStudent);
            _mapper.Setup(s => s.Map<Student>(updatedStudent)).Returns(expected);

            //Act
            var actual = _studentsService.Update(expected.Id, expected);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Delete_StudentWithIdNotExist_ShouldThrowsStudentNotFoundException()
        {
            //Arrange
            var nonExistingStudentId = 3;

            try
            {
                //Act
                _studentsService.Delete(nonExistingStudentId);
            }
            catch (StudentNotFoundException)
            {
                //Assert
                Assert.Throws<StudentNotFoundException>(() => _studentsService.Delete(nonExistingStudentId));
            }
        }

        [TestCase("romankozlov@gmail.com", true)]
        [TestCase("romankozlov@gmail.com", false)]
        public void IsUniqueEmail_ShouldReturnTrueOrFalse(string email, bool expected)
        {
            //Arrange
            _studentRepository.Setup(s => s.IsUniqueEmail(email)).Returns(expected);

            //Act
            var actual = _studentsService.IsUniqueEmail(email);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestCase("89169475896", true)]
        [TestCase("89169475896", false)]
        public void IsUniquePhoneNumber_ShouldReturnTrueOrFalse(string phoneNumber, bool expected)
        {
            //Arrange
            _studentRepository.Setup(s => s.IsUniquePhoneNumber(phoneNumber)).Returns(expected);

            //Act
            var actual = _studentsService.IsUniquePhoneNumber(phoneNumber);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        private static readonly List<Student> _students = new()
        {
            new Student { Id = 1, FullName = "Roman Kozlov", Email = "romankozlov@gmail.com", PhoneNumber = "89169475896", Group = new Group { Id = 1 } },
            new Student { Id = 2, FullName = "Igor Sokolov", Email = "igorsokolov@gmail.com", PhoneNumber = "88329445692", Group = new Group { Id = 2 } }
        };

        private static readonly List<StudentDb> _dbStudents = new()
        {
            new StudentDb { Id = 1, FullName = "Roman Kozlov", Email = "romankozlov@gmail.com", PhoneNumber = "89169475896", GroupId = 1 },
            new StudentDb { Id = 2, FullName = "Igor Sokolov", Email = "igorsokolov@gmail.com", PhoneNumber = "88329445692", GroupId = 2 }
        };

        private readonly PageParams _blPageParams = new() { CurrentPage = 1, PageSize = 2 };
        private readonly DataAccess.PageParams _daPageParams = new() { CurrentPage = 1, PageSize = 2 };

        private static readonly object[] _getStudentCases =
        {
            new object[] { 1, _students[0] },
            new object[] { 4, null }
        };

        private static readonly object[] _getAllStudentsCases =
        {
            new object[]
            {
                new DataAccess.Page<StudentDb>() { Data = _dbStudents, TotalPages = 1 },
                new Page<Student>() { Data = _students, TotalPages = 1 }
            },
            new object[] { new DataAccess.Page<StudentDb>(),  new Page<Student>() }
        };

        private static readonly object[] _getAllStudentsByGroupIdCases =
        {
            new object[] { new List<StudentDb>() { _dbStudents[0] }, 1, new List<Student>() { _students[0] } },
            new object[] { new List<StudentDb>(), 4, new List<Student>()  },
            new object[] { new List<StudentDb>(), null, new List<Student>() }
        };

        private static readonly object[] _getAllStudentsByFilterCases =
        {
            new object[] { 1, null, null, null, null,
                new DataAccess.Page<StudentDb>() { Data = new List<StudentDb>() { _dbStudents[0] }, TotalPages = 1 },
                new Page<Student>() { Data = new List<Student>() { _students[0] }, TotalPages = 1 }
            },
            new object[] { null, "Roman Kozlov", null, null, null,
                new DataAccess.Page<StudentDb>() { Data = new List<StudentDb>() { _dbStudents[0] }, TotalPages = 1 },
                new Page<Student>() { Data = new List<Student>() { _students[0] }, TotalPages = 1 }
            },
            new object[] { null, null, "romankozlov@gmail.com", null, null,
                new DataAccess.Page<StudentDb>() { Data = new List<StudentDb>() { _dbStudents[0] }, TotalPages = 1 },
                new Page<Student>() { Data = new List<Student>() { _students[0] }, TotalPages = 1 }
            },
            new object[] { null, null, null, "89169475896", null,
                new DataAccess.Page<StudentDb>() { Data = new List<StudentDb>() { _dbStudents[0] }, TotalPages = 1 },
                new Page<Student>() { Data = new List<Student>() { _students[0] }, TotalPages = 1 }
            },
            new object[] { null, null, null, null, "6501",
                new DataAccess.Page<StudentDb>() { Data = new List<StudentDb>() { _dbStudents[0] }, TotalPages = 1 },
                new Page<Student>() { Data = new List<Student>() { _students[0] }, TotalPages = 1 }
            },
            new object[] { null, null, null, null, null,
                new DataAccess.Page<StudentDb>() { Data = _dbStudents, TotalPages = 1 },
                new Page<Student>() { Data = _students, TotalPages = 1 }
            },
            new object[] { 3, null, null, null, null,
                new DataAccess.Page<StudentDb>(),
                new Page<Student>()
            }
        };
    }
}
