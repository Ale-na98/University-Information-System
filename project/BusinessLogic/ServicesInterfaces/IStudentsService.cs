using DataAccess;
using System.Collections.Generic;

namespace BusinessLogic
{
    public interface IStudentsService
    {
        int Create(StudentDto studentDto);
        StudentDto Get(int studentId);
        IReadOnlyCollection<StudentDto> GetAll();
        int Update(int studentId, StudentDto studentDto);
        void Delete(int studentId);
    }
}