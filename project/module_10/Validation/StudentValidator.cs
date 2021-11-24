using BusinessLogic;
using FluentValidation;

namespace module_10
{
    public class StudentValidator : AbstractValidator<StudentDto>
    {
        public StudentValidator()
        {
            RuleFor(x => x.FullName).NotNull().Length(3, 30).WithMessage("The full name length must be inclusive between 3 and 30 symbols.");
            RuleFor(x => x.Email).NotNull().EmailAddress().WithMessage("The email address must be provided in the required format (e.g. fullname@mail.com).");
            RuleFor(x => x.PhoneNumber).NotNull().MaximumLength(18).WithMessage("The phone number length must be no longer than 18 numbers.");
        }
    }
}