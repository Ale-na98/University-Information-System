using System;
using BusinessLogic;
using FluentValidation;

namespace module_10
{
    public class AttendanceValidator : AbstractValidator<AttendanceDto>
    {
        public AttendanceValidator()
        {
            RuleFor(x => x.LectureDate).Must(x => x != default).WithMessage("The date must be provided in format YYYY-MM-DD.");
        }
    }
}