using AutoMapper;
using BusinessLogic.Domain;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Presentation.DataTransferObjects.Students;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("/api/students")]
    public class StudentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService, IMapper mapper)
        {
            _mapper = mapper;
            _studentService = studentService;
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateStudentForm student)
        {
            var createdStudent = _studentService.Create(_mapper.Map<Student>(student));
            return Created($"api/students/{createdStudent.Id}", createdStudent);
        }

        [HttpGet("{id}")]
        public ActionResult<StudentDetailsViewModel> GetWithGroup(int id)
        {
            var student = _studentService.GetWithGroup(id);
            return Ok(_mapper.Map<StudentDetailsViewModel>(student));
        }

        [HttpGet("all.{format}"), FormatFilter]
        public ActionResult<IList<StudentDetailsViewModel>> GetAllWithGroups([FromQuery] PageParams parameters)
        {
            var students = _studentService.GetAllWithGroups(parameters);
            return Ok(_mapper.Map<IList<StudentDetailsViewModel>>(students.Data));
        }

        [HttpGet("groups/{groupId}.{format}"), FormatFilter]
        public ActionResult<IList<StudentResponse>> GetAllByGroupId(int groupId)
        {
            var students = _studentService.GetAllByGroupId(groupId);
            return Ok(_mapper.Map<IList<StudentResponse>>(students));
        }

        [HttpGet("filtered.{format}"), FormatFilter]
        public ActionResult<IList<StudentDetailsViewModel>> GetByFilter(int? id, string fullName, string email,
            string phoneNumber, string group, [FromQuery] PageParams parameters)
        {
            var students = _studentService.GetByFilter(id, fullName, email, phoneNumber, group, parameters);
            return Ok(_mapper.Map<IList<StudentDetailsViewModel>>(students.Data));
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] EditStudentForm student)
        {
            var updatedStudent = _studentService.Update(id, _mapper.Map<Student>(student) with { Id = id });
            return Ok(updatedStudent);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _studentService.Delete(id);
            return NoContent();
        }
    }
}
