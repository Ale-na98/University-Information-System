using DataAccess;
using System.Collections.Generic;

namespace BusinessLogic
{
    public interface ITeachersService
    {
        int Create(TeacherDto teacherDto);
        TeacherDto Get(int teacherId);
        IReadOnlyCollection<TeacherDto> GetAll();
        int Update(int teacherId, TeacherDto teacherDto);
        void Delete(int teacherId);
    }
}