using System;

namespace BusinessLogic
{
    public class StudentNotFoundException : BusinessException
    {
        public StudentNotFoundException(string message) : base(message) { }
    }
}