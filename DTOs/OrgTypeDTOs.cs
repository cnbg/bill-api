namespace billing.DTOs;

public record OrgTypeDto(Guid Id, string Name);

public record CreateOrgTypeRequest(string Name);

public record UpdateOrgTypeRequest(string? Name);
