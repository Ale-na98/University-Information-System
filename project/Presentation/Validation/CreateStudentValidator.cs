using FluentValidation;
using BusinessLogic.Services;
using Presentation.DataTransferObjects.Students;

namespace Presentation.Validation
{
    public class CreateStudentValidator : AbstractValidator<CreateStudentForm>
    {
        private readonly StudentService _studentService;
        private readonly GroupService _groupService;

        public CreateStudentValidator(StudentService studentService, GroupService groupService)
        {
            _studentService = studentService;
            _groupService = groupService;

            RuleFor(student => student.FullName)
                .NotEmpty()
                    .WithMessage(ValidationError.EmptyFieldError);

            RuleFor(student => student.Email)
                .NotEmpty()
                    .WithMessage(ValidationError.EmptyFieldError)
                .EmailAddress()
                    .WithMessage(ValidationError.EmailFormatError)
                .Must(IsUniqueEmail)
                    .WithMessage(ValidationError.UniqueEmailError);

            RuleFor(student => student.PhoneNumber)
                .NotEmpty()
                    .WithMessage(ValidationError.EmptyFieldError)
                .Must(IsUniquePhoneNumber)
                    .WithMessage(ValidationError.UniquePhoneNumberError);

            RuleFor(student => student.GroupId)
                .NotEmpty()
                    .WithMessage(ValidationError.EmptyFieldError)
                .Must(IsGroupExist)
                    .WithMessage(ValidationError.ExistGroupError);
        }

        private bool IsUniqueEmail(string email)
        {
            return _studentService.IsUniqueEmail(email);
        }

        private bool IsUniquePhoneNumber(string phoneNumber)
        {
            return _studentService.IsUniquePhoneNumber(phoneNumber);
        }

        private bool IsGroupExist(int groupId)
        {
            return _groupService.IsGroupExist(groupId);
        }
    }
}
