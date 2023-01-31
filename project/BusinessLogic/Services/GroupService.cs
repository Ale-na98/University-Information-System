using AutoMapper;
using System.Linq;
using DataAccess.Entities;
using BusinessLogic.Domain;
using DataAccess.Interfaces;
using System.Collections.Generic;

namespace BusinessLogic.Services
{
    public class GroupService
    {
        private readonly IMapper _mapper;
        private readonly IUniversityRepository<GroupDb> _groupRepository;

        public GroupService(IUniversityRepository<GroupDb> groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        public IList<Group> GetAll()
        {
            var groupsDb = _groupRepository.GetAll();
            if (!groupsDb.Any())
            {
                return new List<Group>();
            }
            return _mapper.Map<IList<Group>>(groupsDb);
        }

        public bool IsGroupExist(int id)
        {
            return _groupRepository.Get(id) != null;
        }
    }
}
