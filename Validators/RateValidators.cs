using billing.DTOs;
using FluentValidation;

namespace billing.Validators;

public class CreateRateValidator : AbstractValidator<CreateRateRequest>
{
    public CreateRateValidator(AppDbCtx db)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class UpdateRateValidator : AbstractValidator<UpdateRateRequest>
{
    public UpdateRateValidator()
    {
        RuleFor(x => x.Name).MaximumLength(200);
    }
}
