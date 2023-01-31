using DataAccess.Entities;
using System.Collections.Generic;

namespace DataAccess.Interfaces
{
    public interface IStudentRepository : IUniversityRepository<StudentDb>
    {
        StudentDb GetWithGroup(int id);
        Page<StudentDb> GetAllWithGroups(PageParams parameters);
        IList<StudentDb> GetAllByGroupId(int? groupId);
        Page<StudentDb> GetByFilter(int? studentId, string fullName, string email,
            string phoneNumber, string group, PageParams parameters);
        bool IsUniqueEmail(string email);
        bool IsUniquePhoneNumber(string phoneNumber);
    }
}
