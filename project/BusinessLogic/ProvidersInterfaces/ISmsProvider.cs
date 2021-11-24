namespace BusinessLogic
{
    public interface ISmsProvider
    {
        string SendSms(int lectureId, int studentId);
    }
}