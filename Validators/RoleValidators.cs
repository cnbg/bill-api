using System.Data;
using billing.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace billing.Validators;

public class CreateRoleValidator : AbstractValidator<CreateRoleRequest>
{
    public CreateRoleValidator(AppDbCtx db)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class UpdateRoleValidator : AbstractValidator<UpdateRoleRequest>
{
    public UpdateRoleValidator()
    {
        RuleFor(x => x.Name).MaximumLength(200);
    }
}

public class UpdateRolePermValidator : AbstractValidator<UpdateRolePermRequest>
{
    public UpdateRolePermValidator()
    {
        RuleFor(x => x.Perm).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Add).NotNull();
    }
}
