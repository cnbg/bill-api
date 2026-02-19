namespace billing.DTOs;

public record OrgDto(Guid Id, string Name);
public record CreateOrgRequest(string Name);

public record UpdateOrgRequest(string? Name);
