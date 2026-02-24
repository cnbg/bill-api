using billing.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace billing.Validators;

public class CreateClientValidator : AbstractValidator<CreateClientRequest>
{
    public CreateClientValidator(AppDbCtx db)
    {
        RuleFor(x => x.Account).NotEmpty().MaximumLength(100);
    }
}

public class UpdateClientValidator : AbstractValidator<UpdateClientRequest>
{
    public UpdateClientValidator()
    {
        RuleFor(x => x.Account).MaximumLength(100);
    }
}
