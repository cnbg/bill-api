using billing.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace billing.Validators;

public class CreateOrgTypeValidator : AbstractValidator<CreateOrgTypeRequest>
{
    public CreateOrgTypeValidator(AppDbCtx db)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class UpdateOrgTypeValidator : AbstractValidator<UpdateOrgTypeRequest>
{
    public UpdateOrgTypeValidator()
    {
        RuleFor(x => x.Name).MaximumLength(200);
    }
}
