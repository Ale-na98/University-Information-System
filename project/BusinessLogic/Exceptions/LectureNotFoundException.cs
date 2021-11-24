using System;

namespace BusinessLogic
{
    public class LectureNotFoundException : BusinessException
    {
        public LectureNotFoundException(string message) : base(message) { }
    }
}