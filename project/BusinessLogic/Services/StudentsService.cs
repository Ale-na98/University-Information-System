using DataAccess;
using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace BusinessLogic
{
    public class StudentsService : IStudentsService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<StudentsService> _logger; 
        private readonly IStudentsRepository _studentsRepository;

        public StudentsService(IStudentsRepository studentsRepository, IMapper mapper, ILogger<StudentsService> logger)
        {
            _studentsRepository = studentsRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public int Create(StudentDto studentDto)
        {
            var studentDb = _mapper.Map<StudentDb>(studentDto);
            var id = _studentsRepository.Create(studentDb);
            _logger.LogInformation($"The new student with id {id} was created.");
            return id;
        }

        public StudentDto Get(int studentId)
        {
            var studentDb = _studentsRepository.Get(studentId);
            if (studentDb == null)
            {
                throw new StudentNotFoundException($"There is no student with id {studentId}.");
            }
            return _mapper.Map<StudentDto>(studentDb);
        }

        public IReadOnlyCollection<StudentDto> GetAll()
        {
            var studentsDb = _studentsRepository.GetAll();
            if (!studentsDb.Any())
            {
                _logger.LogWarning($"There are no students.");
                return new List<StudentDto>();
            }
            return _mapper.Map<IReadOnlyCollection<StudentDto>>(studentsDb);
        }

        public int Update(int studentId, StudentDto studentDto)
        {
            var student = _studentsRepository.Get(studentId);
            if (student == null)
            {
                throw new StudentNotFoundException($"There is no student with id {studentId}.");
            }
            var studentDb = _mapper.Map<StudentDb>(studentDto);
            _studentsRepository.Update(studentDb);
            _logger.LogInformation($"The student with id {studentId} was updated.");
            return studentId;
        }

        public void Delete(int studentId)
        {
            var student = _studentsRepository.Get(studentId);
            if (student == null)
            {
                throw new StudentNotFoundException($"There is no student with id {studentId}.");
            }
            _studentsRepository.Delete(studentId);
        }
    }
}