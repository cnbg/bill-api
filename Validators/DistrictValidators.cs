using billing.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace billing.Validators;

public class CreateDistrictValidator : AbstractValidator<CreateDistrictRequest>
{
    public CreateDistrictValidator(AppDbCtx db)
    {
        RuleFor(x => x.Code).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class UpdateDistrictValidator : AbstractValidator<UpdateDistrictRequest>
{
    public UpdateDistrictValidator()
    {
        RuleFor(x => x.Code).MaximumLength(100);
        RuleFor(x => x.Name).MaximumLength(200);
    }
}
