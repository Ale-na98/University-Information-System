using System;

namespace BusinessLogic
{
    public class TeacherNotFoundException : BusinessException
    {
        public TeacherNotFoundException(string message) : base(message) { }
    }
}