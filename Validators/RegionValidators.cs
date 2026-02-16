using billing.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace billing.Validators;

public class CreateRegionValidator : AbstractValidator<CreateRegionRequest>
{
    public CreateRegionValidator(AppDbCtx db)
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class UpdateRegionValidator : AbstractValidator<UpdateRegionRequest>
{
    public UpdateRegionValidator()
    {
        RuleFor(x => x.Code).MaximumLength(100);
        RuleFor(x => x.Name).MaximumLength(200);
    }
}
