using DataAccess;
using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace BusinessLogic
{
    public class LecturesService : ILecturesService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<LecturesService> _logger;
        private readonly ILecturesRepository _lecturesRepository;

        public LecturesService(ILecturesRepository lecturesRepository, IMapper mapper, ILogger<LecturesService> logger)
        {
            _lecturesRepository = lecturesRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public int Create(string lectureName)
        {
            var lecture = new LectureDb() { Name = lectureName };
            var id = _lecturesRepository.Create(lecture);
            _logger.LogInformation($"The new lecture with id {id} was created.");
            return id;
        }

        public LectureDto Get(int lectureId)
        {
            var lectureDb = _lecturesRepository.Get(lectureId);
            if (lectureDb == null)
            {
                throw new LectureNotFoundException($"There is no lecture with id {lectureId}.");
            }
            return _mapper.Map<LectureDto>(lectureDb);
        }

        public IReadOnlyCollection<LectureDto> GetAll()
        {
            var lecturesDb = _lecturesRepository.GetAll();
            if (!lecturesDb.Any())
            {
                _logger.LogWarning($"There are no lectures.");
                return new List<LectureDto>();
            }
            return _mapper.Map<IReadOnlyCollection<LectureDto>>(lecturesDb);
        }

        public int Update(int lectureId, LectureDto lectureDto)
        {
            var lecture = _lecturesRepository.Get(lectureId);
            if (lecture == null)
            {
                throw new LectureNotFoundException($"There is no lecture with id {lectureId}.");
            }
            var lectureDb = _mapper.Map<LectureDb>(lectureDto);
            _lecturesRepository.Update(lectureDb);
            _logger.LogInformation($"The lecture with id {lectureId} was updated.");
            return lectureId;
        }

        public void Delete(int lectureId)
        {
            var teacher = _lecturesRepository.Get(lectureId);
            if (teacher == null)
            {
                throw new LectureNotFoundException($"There is no lecture with id {lectureId}.");
            }
            _lecturesRepository.Delete(lectureId);
        }
    }
}