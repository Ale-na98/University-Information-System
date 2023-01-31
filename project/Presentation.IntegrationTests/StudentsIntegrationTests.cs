using System;
using System.Net;
using NUnit.Framework;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Json;
using BusinessLogic.Domain;
using System.Threading.Tasks;
using Presentation.DataTransferObjects.Students;

namespace Presentation.IntegrationTests
{
    [TestFixture]
    public class StudentsIntegrationTests
    {
        private HttpClient _client;
        private readonly string _studentUrl = "api/students";
        private ModuleTenWebApplicationFactory _webApplicationFactory;

        [SetUp]
        public void Setup()
        {
            _webApplicationFactory = new ModuleTenWebApplicationFactory();
            _client = _webApplicationFactory.CreateClient();
        }

        [Test]
        public async Task Create_ShouldReturnUrlValueAndSuccessStatusCode()
        {
            // Arrange
            var request = new CreateStudentForm()
            {
                FullName = "Sergey Morozov",
                Email = "sergeymorozov@gmail.com",
                PhoneNumber = "89129478959",
                GroupId = 1
            };

            string expected = Serialize(new Student()
            {
                Id = 13,
                FullName = "Sergey Morozov",
                Email = "sergeymorozov@gmail.com",
                PhoneNumber = "89129478959",
                Group = new Group() { Id = 1, Name = "6501" }
            });

            // Act
            var result = await _client.PostAsJsonAsync(_studentUrl, request);

            // Assert
            result.EnsureSuccessStatusCode(); 
            Assert.IsTrue(result.StatusCode == HttpStatusCode.Created);
            Assert.AreEqual(expected, result.Content.ReadAsStringAsync().Result);
            Assert.AreEqual("api/students/13", result.Headers.Location.ToString());
        }

        [TestCase("", "sergeymorozov@gmail.com", "89129478969", 1)]
        [TestCase("Sergey Morozov", "", "89129478969", 1)]
        [TestCase("Sergey Morozov", "sergeymorozov", "89129478969", 1)]
        [TestCase("Sergey Morozov", "romankozlov@gmail.com", "89129478969", 1)]
        [TestCase("Sergey Morozov", "sergeymorozov@gmail.com", "", 1)]
        [TestCase("Sergey Morozov", "sergeymorozov@gmail.com", "89169475896", 1)]
        [TestCase("Sergey Morozov", "sergeymorozov@gmail.com", "89169475896", 0)]
        [TestCase("Sergey Morozov", "sergeymorozov@gmail.com", "89169475896", 4)]
        public async Task Create_ShouldReturnBadRequestStatusCodeWhenBodyInvalid(string fullName, 
            string email, string phoneNumber, int groupId)
        {
            // Arrange
            var request = new CreateStudentForm()
            {
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber,
                GroupId = groupId
            };

            // Act
            var result = await _client.PostAsJsonAsync(_studentUrl, request);

            // Assert
            Assert.IsTrue(result.StatusCode == HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task Get_ExistentStudentId_ShouldReturnValueAndSuccessStatusCode()
        {
            // Arrange
            string expected = Serialize(new StudentDetailsViewModel()
            {
                Id = 1,
                FullName = "Roman Kozlov",
                Email = "romankozlov@gmail.com",
                PhoneNumber = "89169475896",
                GroupName = "6501"
            });

            // Act
            var result = await _client.GetAsync(_studentUrl + "/1");

            // Assert
            result.EnsureSuccessStatusCode();
            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.AreEqual(expected, result.Content.ReadAsStringAsync().Result);
        }

        [Test]
        public async Task Get_NonExistentStudentId_ShouldReturnNoContentStatusCode()
        {
            var result = await _client.GetAsync(_studentUrl + "/0");
            Assert.IsTrue(result.StatusCode == HttpStatusCode.NoContent);
        }
                
        [Test]
        public async Task Update_ExistentStudentId_ShouldReturnValueAndSuccessStatusCode()
        {
            // Arrange
            var request = new EditStudentForm()
            {
                FullName = "Roman Kozlov",
                Email = "romankozlov@gmail.com",
                PhoneNumber = "89169475896",
                GroupId = 1
            };

            string expected = Serialize(new Student()
            {
                Id = 1,
                FullName = "Roman Kozlov",
                Email = "romankozlov@gmail.com",
                PhoneNumber = "89169475896",
                Group = new Group() { Id = 1, Name = "6501" }
            });

            // Act
            var result = await _client.PutAsJsonAsync(_studentUrl + "/1", request);

            // Assert
            result.EnsureSuccessStatusCode();
            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.AreEqual(expected, result.Content.ReadAsStringAsync().Result);
        }

        [Test]
        public async Task Update_NonExistentStudentId_ShouldReturnInternalServerErrorStatusCode()
        {
            // Arrange
            var request = new EditStudentForm()
            {
                FullName = "Roman Kozlov",
                Email = "romankozlov1@gmail.com",
                PhoneNumber = "89129478969",
                GroupId = 1
            };

            // Act
            var result = await _client.PutAsJsonAsync(_studentUrl + "/0", request);

            // Assert
            Assert.IsTrue(result.StatusCode == HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task Delete_ExistentStudentId_ShouldReturnNoContentStatusCode()
        {
            var result = await _client.DeleteAsync(_studentUrl + "/2");
            result.EnsureSuccessStatusCode();
            Assert.IsTrue(result.StatusCode == HttpStatusCode.NoContent);
        }

        [Test]
        public async Task Delete_NonExistentStudentId_ShouldReturnInternalServerErrorStatusCode()
        {
            var result = await _client.DeleteAsync(_studentUrl + "/0");
            Assert.IsTrue(result.StatusCode == HttpStatusCode.InternalServerError);
        }

        private static string Serialize(Object student)
        {
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            return JsonSerializer.Serialize(student, serializeOptions);
        }
    }
}
