using AutoMapper;
using System.Linq;
using BusinessLogic.Domain;
using DataAccess.Interfaces;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Services
{
    public class LectureService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<LectureService> _logger;
        private readonly ILectureRepository _lectureRepository;

        public LectureService(ILectureRepository lectureRepository,
            IMapper mapper, ILogger<LectureService> logger)
        {
            _lectureRepository = lectureRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public IList<Lecture> GetAllByGroupId(int? groupId)
        {
            var lecturesDb = _lectureRepository.GetAllByGroupId(groupId);
            if (!lecturesDb.Any())
            {
                return new List<Lecture>();
            }
            return _mapper.Map<IList<Lecture>>(lecturesDb);
        }
    }
}
