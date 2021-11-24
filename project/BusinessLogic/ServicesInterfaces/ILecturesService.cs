using DataAccess;
using System.Collections.Generic;

namespace BusinessLogic
{
    public interface ILecturesService
    {
        int Create(string lectureName);
        LectureDto Get(int lectureId);
        IReadOnlyCollection<LectureDto> GetAll();
        int Update(int lectureId, LectureDto lectureDto);
        void Delete(int lectureId);
    }
}