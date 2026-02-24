using billing.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace billing.Validators;

public class CreateChargeValidator : AbstractValidator<CreateChargeRequest>
{
    public CreateChargeValidator(AppDbCtx db)
    {
        RuleFor(x => x.Account).NotEmpty().MaximumLength(200);
    }
}

public class UpdateChargeValidator : AbstractValidator<UpdateChargeRequest>
{
    public UpdateChargeValidator()
    {
        RuleFor(x => x.Account).MaximumLength(200);
    }
}
