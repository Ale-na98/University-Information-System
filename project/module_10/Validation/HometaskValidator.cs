using BusinessLogic;
using FluentValidation;

namespace module_10
{
    public class HometaskValidator : AbstractValidator<HometaskDto>
    {
        public HometaskValidator()
        {
            RuleFor(x => x.Mark).NotEmpty().InclusiveBetween(0, 5).WithMessage("The mark must be inclusive between 0 and 5.");
        }
    }
}