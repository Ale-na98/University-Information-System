namespace DataAccess
{
    public interface IStudentsRepository : IUniversityRepository<StudentDb>
    {
        int Create(StudentDb student);
    }
}