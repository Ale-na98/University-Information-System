namespace DataAccess
{
    public interface IHometasksRepository : IUniversityRepository<HometaskDb>
    {
        int Create(HometaskDb hometask);
        double GetAverageMark(int lectureId, int studentId);
    }
}