using DataAccess;
using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace BusinessLogic
{
    public class TeachersService : ITeachersService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TeachersService> _logger;
        private readonly ITeachersRepository _teachersRepository;

        public TeachersService(ITeachersRepository teachersRepository, IMapper mapper, ILogger<TeachersService> logger)
        {
            _teachersRepository = teachersRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public int Create(TeacherDto teacherDto)
        {
            var teacherDb = _mapper.Map<TeacherDb>(teacherDto);
            var id = _teachersRepository.Create(teacherDb);
            _logger.LogInformation($"The new teacher with {id} was created.");
            return id;
        }

        public TeacherDto Get(int teacherId)
        {
            var teacherDb = _teachersRepository.Get(teacherId);
            if (teacherDb == null)
            {
                throw new TeacherNotFoundException($"There is no teacher with id {teacherId}.");
            }
            return _mapper.Map<TeacherDto>(teacherDb);
        }

        public IReadOnlyCollection<TeacherDto> GetAll()
        {
            var teachersDb = _teachersRepository.GetAll();
            if (!teachersDb.Any())
            {
                _logger.LogWarning($"There are no teachers");
                return new List<TeacherDto>();
            }
            return _mapper.Map<IReadOnlyCollection<TeacherDto>>(teachersDb);
        }

        public int Update(int teacherId, TeacherDto teacherDto)
        {
            var teacher = _teachersRepository.Get(teacherId);
            if (teacher == null)
            {
                throw new TeacherNotFoundException($"There is no teacher with id {teacherId}.");
            }
            var teacherDb = _mapper.Map<TeacherDb>(teacherDto);
            _teachersRepository.Update(teacherDb);
            _logger.LogInformation($"The teacher with id {teacherId} was updated.");
            return teacherId;
        }

        public void Delete(int teacherId)
        {
            var teacher = _teachersRepository.Get(teacherId);
            if (teacher == null)
            {
                throw new TeacherNotFoundException($"There is no teacher with id {teacherId}.");
            }
            _teachersRepository.Delete(teacherId);
        }
    }
}