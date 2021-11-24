using System.Net;
using BusinessLogic;
using NUnit.Framework;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace module_10.IntegrationTests
{
    [TestFixture]
    public class LecturesIntegrationTests
    {
        private ModuleTenWebApplicationFactory _webApplicationFactory;
        private readonly string _lectureUrl = "api/lectures";
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _webApplicationFactory = new ModuleTenWebApplicationFactory();
            _client = _webApplicationFactory.CreateClient();
        }

        [Test]
        public async Task Create_NewLectureJson_RetunsUrlAndStatusCode200()
        {
            // Act
            var result = await _client.PostAsJsonAsync(_lectureUrl + "/English", "English");

            // Assert
            result.EnsureSuccessStatusCode();
            Assert.AreEqual("api/lectures/3", result.Content.ReadAsStringAsync().Result);
        }

        [Test]
        public async Task Get_LectureById_RetunsLectureAndStatusCode200()
        {
            var result = await _client.GetAsync(_lectureUrl + "/1");
            result.EnsureSuccessStatusCode();
            Assert.AreEqual("{\"id\":1,\"name\":\"Maths\",\"teacherId\":1}", result.Content.ReadAsStringAsync().Result);
        }

        [Test]
        public async Task Get_LectureById_RetunsStatusCode500()
        {
            var result = await _client.GetAsync(_lectureUrl + "/0");
            Assert.IsTrue(result.StatusCode == HttpStatusCode.InternalServerError);
        }

        [TestCase("api/lectures")]
        [TestCase("api/lectures/.json")]
        public async Task GetAll_AllLectures_RetunsLecturesAndStatusCode200(string teacherUrl)
        {
            var result = await _client.GetAsync(teacherUrl);
            result.EnsureSuccessStatusCode();
            Assert.AreEqual("[{\"id\":1,\"name\":\"Maths\",\"teacherId\":1}," +
                "{\"id\":2,\"name\":\"Physics\",\"teacherId\":2}]", result.Content.ReadAsStringAsync().Result);
        }

        [Test]
        public async Task Update_LectureIdAndJson_RetunsUrlAndStatusCode200()
        {
            // Arrange
            var request = new LectureDto()
            {
                Name = "Maths",
                TeacherId = 2
            };

            // Act
            var result = await _client.PutAsJsonAsync(_lectureUrl + "/1", request);

            // Assert
            result.EnsureSuccessStatusCode();
            Assert.AreEqual("api/lectures/1", result.Content.ReadAsStringAsync().Result);
        }

        [Test]
        public async Task Update_LectureIdAndJson_RetunsStatusCode500()
        {
            // Arrange
            var request = new LectureDto() 
            { 
                Name = "Maths",
                TeacherId = 1 
            };

            // Act
            var result = await _client.PutAsJsonAsync(_lectureUrl + "/0", request);

            // Assert
            Assert.IsTrue(result.StatusCode == HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task Delete_LectureById_RetunsStatusCode200()
        {
            var result = await _client.DeleteAsync(_lectureUrl + "/2");
            result.EnsureSuccessStatusCode();
            Assert.AreEqual($"The lecture with Id 2 was deleted.", result.Content.ReadAsStringAsync().Result);
        }

        [Test]
        public async Task Delete_LectureById_RetunsStatusCode500()
        {
            var result = await _client.DeleteAsync(_lectureUrl + "/0");
            Assert.IsTrue(result.StatusCode == HttpStatusCode.InternalServerError);
        }
    }
}