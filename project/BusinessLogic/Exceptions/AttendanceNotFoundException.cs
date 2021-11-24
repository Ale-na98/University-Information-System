using System;

namespace BusinessLogic
{
    public class AttendanceNotFoundException : BusinessException
    {
        public AttendanceNotFoundException(string message) : base(message) { }
    }
}