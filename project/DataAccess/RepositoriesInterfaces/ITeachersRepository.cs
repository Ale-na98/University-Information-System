namespace DataAccess
{
    public interface ITeachersRepository : IUniversityRepository<TeacherDb>
    {
        int Create(TeacherDb teacher);
    }
}