using billing.DTOs;
using FluentValidation;

namespace billing.Validators;

public class CreateExpenseValidator : AbstractValidator<CreateExpenseRequest>
{
    public CreateExpenseValidator(AppDbCtx db)
    {
        RuleFor(x => x.Amount).NotEmpty().GreaterThan(0);
    }
}

public class UpdateExpenseValidator : AbstractValidator<UpdateExpenseRequest>
{
    public UpdateExpenseValidator()
    {
        RuleFor(x => x.Amount).NotEmpty().GreaterThan(0);
    }
}
