using System.Net;
using BusinessLogic;
using NUnit.Framework;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace module_10.IntegrationTests
{
    [TestFixture]
    public class StudentsIntegrationTests
    {
        private ModuleTenWebApplicationFactory _webApplicationFactory;
        private readonly string _studentUrl = "api/students";
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _webApplicationFactory = new ModuleTenWebApplicationFactory();
            _client = _webApplicationFactory.CreateClient();
        }

        [Test]
        public async Task Create_NewStudentJson_RetunsUrlAndStatusCode200()
        {
            // Arrange
            var request = new StudentDto()
            {
                FullName = "Sergey Morozov",
                Email = "sergeymorozov@gmail.com",
                PhoneNumber = "89129478969"
            };

            // Act
            var result = await _client.PostAsJsonAsync(_studentUrl, request);

            // Assert
            result.EnsureSuccessStatusCode();
            Assert.AreEqual("api/students/3", result.Content.ReadAsStringAsync().Result);
        }

        [TestCase(" ", "sergeymorozov@gmail.com", "89129478969")]
        [TestCase("Sergey Morozov", "sergeymorozov", "89129478969")]
        [TestCase("Sergey Morozov", "sergeymorozov@gmail.com", "9129478967893515877")]
        public async Task Create_NewStudentJson_RetunsStatusCode400(string fullName, string email, string phoneNumber)
        {
            // Arrange
            var request = new StudentDto()
            {
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber
            };

            // Act
            var result = await _client.PostAsJsonAsync(_studentUrl, request);

            // Assert
            Assert.IsTrue(result.StatusCode == HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task Get_StudentById_RetunsStudentAndStatusCode200()
        {
            var result = await _client.GetAsync(_studentUrl + "/1");
            result.EnsureSuccessStatusCode();
            Assert.AreEqual("{\"id\":1,\"fullName\":\"Roman Kozlov\",\"email\":\"romankozlov@gmail.com\",\"phoneNumber\":\"89169475896\"}", result.Content.ReadAsStringAsync().Result);
        }

        [Test]
        public async Task Get_StudentById_RetunsStatusCode500()
        {
            var result = await _client.GetAsync(_studentUrl + "/0");
            Assert.IsTrue(result.StatusCode == HttpStatusCode.InternalServerError);
        }

        [TestCase("api/students")]
        [TestCase("api/students/.json")]
        public async Task GetAll_AllStudents_RetunsStudentsAndStatusCode200(string studentUrl)
        {
            var result = await _client.GetAsync(studentUrl);
            result.EnsureSuccessStatusCode();
            Assert.AreEqual("[{\"id\":1,\"fullName\":\"Roman Kozlov\",\"email\":\"romankozlov@gmail.com\",\"phoneNumber\":\"89169475896\"}," +
                "{\"id\":2,\"fullName\":\"Maxim Zakharov\",\"email\":\"maximzakharov@gmail.com\",\"phoneNumber\":\"89129475994\"}]", result.Content.ReadAsStringAsync().Result);
        }

        [Test]
        public async Task Update_StudentIdAndJson_RetunsUrlAndStatusCode200()
        {
            // Arrange
            var request = new StudentDto()
            {
                FullName = "Roman Kozlov",
                Email = "romankozlov1@gmail.com",
                PhoneNumber = "89129478969"
            };

            // Act
            var result = await _client.PutAsJsonAsync(_studentUrl + "/1", request);

            // Assert
            result.EnsureSuccessStatusCode();
            Assert.AreEqual("api/students/1", result.Content.ReadAsStringAsync().Result);
        }

        [Test]
        public async Task Update_StudentIdAndJson_RetunsStatusCode500()
        {
            // Arrange
            var request = new StudentDto()
            {
                FullName = "Roman Kozlov",
                Email = "romankozlov1@gmail.com",
                PhoneNumber = "89129478969"
            };

            // Act
            var result = await _client.PutAsJsonAsync(_studentUrl + "/0", request);

            // Assert
            Assert.IsTrue(result.StatusCode == HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task Delete_StudentById_RetunsStatusCode200()
        {
            var result = await _client.DeleteAsync(_studentUrl + "/2");
            result.EnsureSuccessStatusCode();
            Assert.AreEqual($"The student with Id 2 was deleted.", result.Content.ReadAsStringAsync().Result);
        }

        [Test]
        public async Task Delete_StudentById_RetunsStatusCode500()
        {
            var result = await _client.DeleteAsync(_studentUrl + "/0");
            Assert.IsTrue(result.StatusCode == HttpStatusCode.InternalServerError);
        }
    }
}