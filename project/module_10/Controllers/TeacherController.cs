using System.Linq;
using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace module_10
{
    [ApiController]
    [Route("/api/teachers")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeachersService _teachersService;

        public TeacherController(ITeachersService teachersService)
        {
            _teachersService = teachersService;
        }

        [HttpPost]
        public ActionResult<string> Create(TeacherDto teacherDto)
        {
            var id = _teachersService.Create(teacherDto);
            return Ok($"api/teachers/{id}");
        }

        [HttpGet("{id}")]
        public ActionResult<TeacherDto> Get(int id)
        {
            return Ok(_teachersService.Get(id));
        }

        [HttpGet]
        [HttpGet(".{format}"), FormatFilter]
        public ActionResult<IReadOnlyCollection<TeacherDto>> GetAll()
        {
            return Ok(_teachersService.GetAll());
        }

        [HttpPut("{id}")]
        public ActionResult<string> Update(int id, TeacherDto teacherDto)
        {
            _teachersService.Update(id, teacherDto with { Id = id });
            return Ok($"api/teachers/{id}");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _teachersService.Delete(id);
            return Ok($"The teacher with Id {id} was deleted.");
        }
    }
}