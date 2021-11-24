using DataAccess;
using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace BusinessLogic
{
    public class HometasksService : IHometasksService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<HometasksService> _logger;
        private readonly IHometasksRepository _hometasksRepository;

        public HometasksService(IHometasksRepository hometasksRepository, IMapper mapper, ILogger<HometasksService> logger)
        {
            _hometasksRepository = hometasksRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public int Create(HometaskDto hometaskDto)
        {
            var hometaskDb = _mapper.Map<HometaskDb>(hometaskDto);
            var id = _hometasksRepository.Create(hometaskDb);
            _logger.LogInformation($"The new hometask with id {id} was created.");
            return id;
        }

        public HometaskDto Get(int hometaskId)
        {
            var hometaskDb = _hometasksRepository.Get(hometaskId);
            if (hometaskDb == null)
            {
                throw new HometaskNotFoundException($"There is no hometask with id {hometaskId}.");
            }
            return _mapper.Map<HometaskDto>(hometaskDb);
        }

        public IReadOnlyCollection<HometaskDto> GetAll()
        {
            var hometasksDb = _hometasksRepository.GetAll();
            if (!hometasksDb.Any())
            {
                _logger.LogWarning($"There are no hometasks.");
                return new List<HometaskDto>();
            }
            return _mapper.Map<IReadOnlyCollection<HometaskDto>>(hometasksDb);
        }

        public int Update(int hometaskId, HometaskDto hometaskDto)
        {
            var hometask = _hometasksRepository.Get(hometaskId);
            if (hometask == null)
            {
                throw new HometaskNotFoundException($"There is no hometask with id {hometaskId}.");
            }
            var hometaskDb = _mapper.Map<HometaskDb>(hometaskDto);
            _hometasksRepository.Update(hometaskDb);
            _logger.LogInformation($"The hometask with id {hometaskId} was updated.");
            return hometaskId;
        }

        public void Delete(int hometaskId)
        {
            var hometask = _hometasksRepository.Get(hometaskId);
            if (hometask == null)
            {
                throw new HometaskNotFoundException($"There is no hometask with id {hometaskId}.");
            }
            _hometasksRepository.Delete(hometaskId);
        }
    }
}