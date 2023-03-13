using System.Linq;
using DataAccess.Entities;
using DataAccess.Interfaces;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class LectureRepository : UniversityRepository<LectureDb>, ILectureRepository
    {
        private readonly DbSet<LectureDb> _lectureDbSet;
        private readonly DbSet<ScheduleDb> _scheduleDbSet;

        public LectureRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _lectureDbSet = appDbContext.Set<LectureDb>();
            _scheduleDbSet = appDbContext.Set<ScheduleDb>();
        }

        public IList<LectureDb> GetAllByGroupId(int? groupId)
        {
            var lectures = from lecture in _lectureDbSet
                           join schedule in _scheduleDbSet on lecture.Id equals schedule.LectureId
                           where schedule.GroupId == groupId
                           select new LectureDb { Id = lecture.Id, Name = lecture.Name };
            return lectures.ToList();
        }
    }
}
