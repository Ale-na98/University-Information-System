namespace BusinessLogic
{
    public interface IEmailProvider
    {
        string SendEmail(int lectureId, int? teacherId, int studentId);
    }
}