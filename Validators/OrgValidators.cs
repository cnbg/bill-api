using billing.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace billing.Validators;

public class CreateOrgValidator : AbstractValidator<CreateOrgRequest>
{
    public CreateOrgValidator(AppDbCtx db)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class UpdateOrgValidator : AbstractValidator<UpdateOrgRequest>
{
    public UpdateOrgValidator()
    {
        RuleFor(x => x.Name).MaximumLength(200);
    }
}
