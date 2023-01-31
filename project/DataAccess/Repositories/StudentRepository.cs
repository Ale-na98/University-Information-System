using System;
using System.Linq;
using DataAccess.Entities;
using DataAccess.Interfaces;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    internal class StudentRepository : UniversityRepository<StudentDb>, IStudentRepository
    {
        private readonly DbSet<StudentDb> _studentDbSet;

        public StudentRepository(AppContext universityDbContext) : base(universityDbContext)
        {
            _studentDbSet = universityDbContext.Set<StudentDb>();
        }

        public StudentDb GetWithGroup(int id)
        {
            return _studentDbSet
                .Include(student => student.Group)
                .FirstOrDefault(student => student.Id == id);
        }

        public Page<StudentDb> GetAllWithGroups(PageParams parameters)
        {
            var students = _studentDbSet.Include(student => student.Group);

            var page = new Page<StudentDb>
            {
                Data = students.OrderBy(d => d.Id)
                .Skip((parameters.CurrentPage - 1) * parameters.PageSize)
                .Take(parameters.PageSize).ToList(),

                TotalPages = GetTotalPages(students, parameters.PageSize)
            };

            return page;
        }

        public IList<StudentDb> GetAllByGroupId(int? groupId)
        {
            return _studentDbSet
                .Where(student => student.GroupId == groupId)
                .ToList();
        }

        public Page<StudentDb> GetByFilter(int? id, string fullName, string email, string phoneNumber, string group, PageParams parameters)
        {
            IQueryable<StudentDb> selectedStudents = _studentDbSet.Include(student => student.Group);

            if (id != null)
                selectedStudents = selectedStudents.Where(student => student.Id == id);
            if (fullName != null)
                selectedStudents = selectedStudents.Where(student => student.FullName == fullName);
            if (email != null)
                selectedStudents = selectedStudents.Where(student => student.Email == email);
            if (phoneNumber != null)
                selectedStudents = selectedStudents.Where(student => student.PhoneNumber == phoneNumber);
            if (group != null)
                selectedStudents = selectedStudents.Where(student => student.Group.Name == group);

            var page = new Page<StudentDb>
            {
                Data = selectedStudents.OrderBy(student => student.Id)
                .Skip((parameters.CurrentPage - 1) * parameters.PageSize)
                .Take(parameters.PageSize).ToList(),

                TotalPages = GetTotalPages(selectedStudents, parameters.PageSize)
            };

            return page;
        }

        public bool IsUniqueEmail(string email)
        {
            return !_studentDbSet.Any(student => student.Email == email);
        }

        public bool IsUniquePhoneNumber(string phoneNumber)
        {
            return !_studentDbSet.Any(student => student.PhoneNumber == phoneNumber);
        }

        private int GetTotalPages(IQueryable<StudentDb> selectedStudents, int pageSize)
        {
            var count = selectedStudents.Count();
            return (int)Math.Ceiling(decimal.Divide(count, pageSize));
        }
    }
}
