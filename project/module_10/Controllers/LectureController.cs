using System.Linq;
using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace module_10
{
    [ApiController]
    [Route("/api/lectures")]
    public class LectureController : ControllerBase
    {
        private readonly ILecturesService _lecturesService;

        public LectureController(ILecturesService lecturesService)
        {
            _lecturesService = lecturesService;
        }

        [HttpPost("{name}")]
        public ActionResult<string> Create(string name)
        {
            var id = _lecturesService.Create(name);
            return Ok($"api/lectures/{id}");
        }

        [HttpGet("{id}")]
        public ActionResult<LectureDto> Get(int id)
        {
            return Ok(_lecturesService.Get(id));
        }

        [HttpGet]
        [HttpGet(".{format}"), FormatFilter]
        public ActionResult<IReadOnlyCollection<LectureDto>> GetAll()
        {
            return Ok(_lecturesService.GetAll());
        }

        [HttpPut("{id}")]
        public ActionResult<string> Update(int id, LectureDto lectureDto)
        {
            _lecturesService.Update(id, lectureDto with { Id = id });
            return Ok($"api/lectures/{id}");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _lecturesService.Delete(id);
            return Ok($"The lecture with Id {id} was deleted.");
        }
    }
}