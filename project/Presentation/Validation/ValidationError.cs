namespace Presentation.Validation
{
    public class ValidationError
    {
        public static string EmptyFieldError { get; } = "This field is required.";
        public static string EmailFormatError { get; } = "The email address must be provided in the required format (e.g. fullname@mail.com).";
        public static string UniqueEmailError { get; } = "Such email address already exists, please choose a unique one.";
        public static string UniquePhoneNumberError { get; } = "Such phone number already exists, please choose a unique one.";
        public static string ExistGroupError { get; } = "Such group does not exist.";
    }
}
