using AutoMapper;
using System.Linq;
using DataAccess.Entities;
using BusinessLogic.Domain;
using DataAccess.Interfaces;
using BusinessLogic.Exceptions;
using DataAccess.Elasticsearch;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

using DataAccess.Elasticsearch.Documents;

namespace BusinessLogic.Services
{
    public class StudentService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<StudentService> _logger;
        private readonly IStudentRepository _studentRepository;
        private readonly IUniversityRepository<GroupDb> _groupRepository;
        private readonly ElasticsearchRepository<StudentDocument> _elasticsearchRepository;

        public StudentService(IStudentRepository studentsRepository, IUniversityRepository<GroupDb> groupRepository,
            ElasticsearchRepository<StudentDocument> elasticsearchRepository, IMapper mapper, ILogger<StudentService> logger)
        {
            _elasticsearchRepository = elasticsearchRepository;
            _studentRepository = studentsRepository;
            _groupRepository = groupRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Student Create(Student student)
        {
            var createdStudent = _studentRepository.Create(new StudentDb()
            {
                FullName = student.FullName,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                GroupId = student.Group.Id
            });

            var studentDocument = _mapper.Map<StudentDocument>(createdStudent);
            _elasticsearchRepository.SaveSingle(studentDocument);

            _logger.LogInformation("The new student with id {} was created and indexed.", createdStudent.Id);

            return _mapper.Map<Student>(createdStudent);
        }

        public Student GetWithGroup(int id)
        {
            var studentDb = _studentRepository.GetWithGroup(id);
            if (studentDb == null)
            {
                return null;
            }
            return _mapper.Map<Student>(studentDb);
        }

        public Page<Student> GetAllWithGroups(PageParams parameters)
        {
            var dbParams = _mapper.Map<DataAccess.PageParams>(parameters);
            var studentsDb = _studentRepository.GetAllWithGroups(dbParams);
            if (studentsDb.Data == null)
            {
                return new Page<Student>();
            }
            return _mapper.Map<Page<Student>>(studentsDb);
        }

        public IList<Student> GetAllByGroupId(int? groupId)
        {
            var studentsDb = _studentRepository.GetAllByGroupId(groupId);
            if (!studentsDb.Any())
            {
                return new List<Student>();
            }
            return _mapper.Map<IList<Student>>(studentsDb);
        }

        public Page<Student> GetByFilter(int? id, string fullName, string email,
            string phoneNumber, string group, PageParams parameters)
        {
            var dbParams = _mapper.Map<DataAccess.PageParams>(parameters);
            var studentsDb = _studentRepository.GetByFilter(id, fullName, email, phoneNumber, group, dbParams);
            if (studentsDb.Data == null)
            {
                return new Page<Student>();
            }
            return _mapper.Map<Page<Student>>(studentsDb);
        }

        public Student Update(int id, Student student)
        {
            EnsureStudentExist(id);

            var studentDb = _mapper.Map<StudentDb>(student);
            var updatedStudent = _studentRepository.Update(studentDb);
            var groupName = _groupRepository.Get(updatedStudent.Group.Id).Name;

            updatedStudent.Group.Name = groupName;
            var studentDocument = _mapper.Map<StudentDocument>(updatedStudent);
            _elasticsearchRepository.Update(studentDocument);

            _logger.LogInformation("The student with Id {} was updated and indexed.", updatedStudent.Id);

            return _mapper.Map<Student>(updatedStudent);
        }

        public void Delete(int id)
        {
            var studentDb = EnsureStudentExist(id);
            _studentRepository.Delete(id);

            var studentDocument = _mapper.Map<StudentDocument>(studentDb);
            _elasticsearchRepository.DeleteSingle(studentDocument);

            _logger.LogInformation("The student with Id {} was deleted.", studentDb.Id);
        }

        public bool IsUniqueEmail(string email)
        {
            return _studentRepository.IsUniqueEmail(email);
        }

        public bool IsUniquePhoneNumber(string phoneNumber)
        {
            return _studentRepository.IsUniquePhoneNumber(phoneNumber);
        }

        private StudentDb EnsureStudentExist(int id)
        {
            var studentDb = _studentRepository.Get(id);
            if (studentDb == null)
            {
                throw new StudentNotFoundException($"There is no student with Id {id}.");
            }
            return studentDb;
        }
    }
}
