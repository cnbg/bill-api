using billing.DTOs;
using FluentValidation;

namespace billing.Validators;

public class CreateBalanceValidator : AbstractValidator<CreateBalanceRequest>
{
    public CreateBalanceValidator(AppDbCtx db)
    {
        RuleFor(x => x.Month).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Year).NotEmpty().GreaterThan(0);
    }
}

public class UpdateBalanceValidator : AbstractValidator<UpdateBalanceRequest>
{
    public UpdateBalanceValidator()
    {
        RuleFor(x => x.Month).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Year).NotEmpty().GreaterThan(0);
    }
}
