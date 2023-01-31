using System;
using BusinessLogic.Exceptions;

namespace BusinessLogic
{
    public class LectureNotFoundException : BusinessException
    {
        public LectureNotFoundException(string message) : base(message) { }
    }
}