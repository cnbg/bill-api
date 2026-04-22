namespace billing.DTOs;

public record OrgTypeDto(Guid Id, string Name, bool IsActive);

public record CreateOrgTypeRequest(string Name, bool? IsActive);

public record UpdateOrgTypeRequest(string? Name, bool? IsActive);
