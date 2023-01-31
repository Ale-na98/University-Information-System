using DataAccess.Entities;
using System.Collections.Generic;

namespace DataAccess.Interfaces
{
    public interface ILectureRepository : IUniversityRepository<LectureDb>
    {
        IList<LectureDb> GetAllByGroupId(int? groupId);
    }
}
