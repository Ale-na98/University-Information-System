using DataAccess.Entities;

namespace DataAccess.Interfaces
{
    public interface IHometaskRepository : IUniversityRepository<HometaskDb>
    {
        double GetAverageMark(int lectureId, int studentId);
    }
}
