using BusinessLogic;
using FluentValidation;

namespace module_10
{
    public class TeacherValidator : AbstractValidator<TeacherDto>
    {
        public TeacherValidator()
        {
            RuleFor(x => x.FullName).NotNull().Length(3, 30).WithMessage("The full name length must be inclusive between 3 and 30 symbols.");
            RuleFor(x => x.Email).NotNull().EmailAddress().WithMessage("The email address must be provided in the required format (e.g. fullname@mail.com).");
        }
    }
}