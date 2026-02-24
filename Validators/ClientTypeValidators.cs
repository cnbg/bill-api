using billing.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace billing.Validators;

public class CreateClientTypeValidator : AbstractValidator<CreateClientTypeRequest>
{
    public CreateClientTypeValidator(AppDbCtx db)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class UpdateClientTypeValidator : AbstractValidator<UpdateClientTypeRequest>
{
    public UpdateClientTypeValidator()
    {
        RuleFor(x => x.Name).MaximumLength(200);
    }
}
