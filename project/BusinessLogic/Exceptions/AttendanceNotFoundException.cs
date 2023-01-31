namespace BusinessLogic.Exceptions
{
    internal class AttendanceNotFoundException : BusinessException
    {
        public AttendanceNotFoundException(string message) : base(message) { }
    }
}
