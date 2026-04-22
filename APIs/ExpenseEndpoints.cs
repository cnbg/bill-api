using billing.Constants;
using billing.DTOs;
using billing.Extensions;
using billing.Filters;
using billing.Helpers;
using billing.Services;
using Microsoft.AspNetCore.Mvc;

namespace billing.APIs;

public static class ExpenseEndpoints
{
    public static RouteGroupBuilder MapExpenseEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("v1/Expense")
            .WithExceptionHandler(new HttpExceptionFilter())
            .WithTags("Expense");

        group.MapGet("list", async (
                IExpenseService expenseService,
                MyDataSourceLoadOptions loadOptions
            ) => await expenseService.GetExpenseListAsync(loadOptions))
            .RequirePermission(Permissions.ExpenseView)
            .WithName("GetExpenseList");

        group.MapGet("show/{id:guid}", async (
                Guid id,
                IExpenseService expenseService
            ) => await expenseService.GetExpenseByIdAsync(id))
            .RequirePermission(Permissions.ExpenseView)
            .WithName("GetExpenseById");

        group.MapPost("create", async (
                IExpenseService expenseService,
                [FromBody] CreateExpenseRequest createExpenseDto
            ) => await expenseService.CreateExpenseAsync(createExpenseDto))
            .RequirePermission(Permissions.ExpenseCreate)
            .WithValidation<CreateExpenseRequest>()
            .WithName("CreateExpense");

        group.MapPut("update/{id:guid}", async (
                Guid id,
                IExpenseService expenseService,
                [FromBody] UpdateExpenseRequest updateExpenseDto
            ) =>
            {
                await expenseService.UpdateExpenseAsync(id, updateExpenseDto);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.ExpenseEdit)
            .WithValidation<UpdateExpenseRequest>()
            .WithName("UpdateExpense");

        group.MapDelete("delete/{id:guid}", async (
                IExpenseService expenseService,
                Guid id
            ) =>
            {
                await expenseService.DeleteExpenseAsync(id);
                return Results.NoContent();
            })
            .RequirePermission(Permissions.ExpenseDelete)
            .WithName("DeleteExpense");

        return group;
    }
}
