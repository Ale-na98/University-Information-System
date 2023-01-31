namespace BusinessLogic.Exceptions
{
    public class StudentNotFoundException : BusinessException
    {
        public StudentNotFoundException(string message) : base(message) { }
    }
}
