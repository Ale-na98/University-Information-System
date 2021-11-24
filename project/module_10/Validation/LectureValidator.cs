using BusinessLogic;
using FluentValidation;

namespace module_10
{
    public class LectureValidator : AbstractValidator<LectureDto>
    {
        public LectureValidator()
        {
            RuleFor(x => x.Name).NotNull().Length(2, 30).WithMessage("The name length must be inclusive between 2 and 30 symbols.");
        }
    }
}