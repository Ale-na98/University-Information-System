namespace DataAccess
{
    public interface ILecturesRepository : IUniversityRepository<LectureDb>
    {
        int Create(LectureDb lecture);
    }
}
