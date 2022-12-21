using FluentValidation;
using SAS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAS.Validators
{
    public class CourseDTOValidator: AbstractValidator<CourseDTO>
    {
        public CourseDTOValidator()
        {
            RuleFor(x => x.CourseName).NotNull().MinimumLength(3);
            RuleFor(x => x.CourseDescription).NotNull();
            RuleFor(x => x.CourseImgUrl).NotNull();
        }
    }
}
