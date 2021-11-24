using System.Linq;
using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace module_10
{
    [ApiController]
    [Route("/api/students")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentsService _studentsService;

        public StudentController(IStudentsService studentsService)
        {
            _studentsService = studentsService;
        }

        [HttpPost]
        public ActionResult<string> Create(StudentDto studentDto)
        {
            var id = _studentsService.Create(studentDto);
            return Ok($"api/students/{id}");
        }

        [HttpGet("{id}")]
        public ActionResult<StudentDto> Get(int id)
        {
            return Ok(_studentsService.Get(id));
        }

        [HttpGet]
        [HttpGet(".{format}"), FormatFilter]
        public ActionResult<IReadOnlyCollection<StudentDto>> GetAll()
        {
            return Ok(_studentsService.GetAll());
        }

        [HttpPut("{id}")]
        public ActionResult<string> Update(int id, StudentDto studentDto)
        {
            _studentsService.Update(id, studentDto with { Id = id });
            return Ok($"api/students/{id}");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _studentsService.Delete(id);
            return Ok($"The student with Id {id} was deleted.");
        }
    }
}