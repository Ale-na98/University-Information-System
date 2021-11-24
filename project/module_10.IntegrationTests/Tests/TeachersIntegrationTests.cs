using System.Net;
using BusinessLogic;
using NUnit.Framework;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace module_10.IntegrationTests
{
    [TestFixture]
    public class TeachersIntegrationTests
    {
        private ModuleTenWebApplicationFactory _webApplicationFactory;
        private readonly string _teacherUrl = "api/teachers";
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _webApplicationFactory = new ModuleTenWebApplicationFactory();
            _client = _webApplicationFactory.CreateClient();
        }

        [Test]
        public async Task Create_NewTeacherJson_RetunsUrlAndStatusCode200()
        {
            // Arrange
            var request = new TeacherDto()
            {
                FullName = "Sergey Morozov",
                Email = "sergeymorozov@gmail.com"
            };

            // Act
            var result = await _client.PostAsJsonAsync(_teacherUrl, request);

            // Assert
            result.EnsureSuccessStatusCode();
            Assert.AreEqual("api/teachers/3", result.Content.ReadAsStringAsync().Result);
        }

        [TestCase(" ", "sergeymorozov@gmail.com")]
        [TestCase("Sergey Morozov", "sergeymorozov")]
        public async Task Create_NewTeacherJson_RetunsStatusCode400(string fullName, string email)
        {
            // Arrange
            var request = new TeacherDto()
            {
                FullName = fullName,
                Email = email
            };

            // Act
            var result = await _client.PostAsJsonAsync(_teacherUrl, request);

            // Assert
            Assert.IsTrue(result.StatusCode == HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task Get_TeacherById_RetunsTeacherAndStatusCode200()
        {
            var result = await _client.GetAsync(_teacherUrl + "/1");
            result.EnsureSuccessStatusCode();
            Assert.AreEqual("{\"id\":1,\"fullName\":\"Ivan Ivanov\",\"email\":\"ivanivanov@gmail.com\"}", result.Content.ReadAsStringAsync().Result);
        }

        [Test]
        public async Task Get_TeacherById_RetunsStatusCode500()
        {
            var result = await _client.GetAsync(_teacherUrl + "/0");
            Assert.IsTrue(result.StatusCode == HttpStatusCode.InternalServerError);
        }

        [TestCase("api/teachers")]
        [TestCase("api/teachers/.json")]
        public async Task GetAll_AllTeachers_RetunsTeachersAndStatusCode200(string teacherUrl)
        {
            var result = await _client.GetAsync(teacherUrl);
            result.EnsureSuccessStatusCode();
            Assert.AreEqual("[{\"id\":1,\"fullName\":\"Ivan Ivanov\",\"email\":\"ivanivanov@gmail.com\"}," +
                "{\"id\":2,\"fullName\":\"Peter Petrov\",\"email\":\"peterpetrov@gmail.com\"}]", result.Content.ReadAsStringAsync().Result);
        }

        [Test]
        public async Task Update_TeacherIdAndJson_RetunsUrlAndStatusCode200()
        {
            // Arrange
            var request = new TeacherDto()
            {
                FullName = "Ivan Ivanov",
                Email = "ivanivanov@gmail.com"
            };

            // Act
            var result = await _client.PutAsJsonAsync(_teacherUrl + "/1", request);

            // Assert
            result.EnsureSuccessStatusCode();
            Assert.AreEqual("api/teachers/1", result.Content.ReadAsStringAsync().Result);
        }

        [Test]
        public async Task Update_TeacherIdAndJson_RetunsStatusCode500()
        {
            // Arrange
            var request = new TeacherDto()
            {
                FullName = "Ivan Ivanov",
                Email = "ivanivanov@gmail.com"
            };

            // Act
            var result = await _client.PutAsJsonAsync(_teacherUrl + "/0", request);

            // Assert
            Assert.IsTrue(result.StatusCode == HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task Delete_TeacherById_RetunsStatusCode200()
        {
            var result = await _client.DeleteAsync(_teacherUrl + "/2");
            result.EnsureSuccessStatusCode();
            Assert.AreEqual($"The teacher with Id 2 was deleted.", result.Content.ReadAsStringAsync().Result);
        }

        [Test]
        public async Task Delete_TeacherById_RetunsStatusCode500()
        {
            var result = await _client.DeleteAsync(_teacherUrl + "/0");
            Assert.IsTrue(result.StatusCode == HttpStatusCode.InternalServerError);
        }
    }
}