namespace billing.DTOs;

public record RoleDto(Guid Id, string Name, bool IsActive, Guid OrgId, List<string> Perms, string? Note);

public record CreateRoleRequest(bool? IsActive, string Name, string? Note);

public record UpdateRoleRequest(bool? IsActive, string? Name,  string? Note);

public record UpdateRolePermRequest(string Perm, bool Add);
