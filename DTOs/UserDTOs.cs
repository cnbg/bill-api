namespace billing.DTOs;

public record UserDto(
    Guid Id,
    Guid? OrgId,
    OrgDto? Org,
    string FirstName,
    string LastName,
    string? Email,
    string Type
);

public record CreateUserRequest(
    Guid OrgId,
    string FirstName,
    string LastName,
    string Email
);

public record UpdateUserRequest(
    Guid? OrgId,
    string? FirstName,
    string? LastName,
    string? Email
);
