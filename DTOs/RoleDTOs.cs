namespace billing.DTOs;

public record RoleDto(Guid Id, string Name, bool IsActive, Guid OrgId, List<string> Perms);

public record CreateRoleRequest(bool? IsActive, string Name);

public record UpdateRoleRequest(bool? IsActive, string? Name);

public record UpdateRolePermRequest(string Perm, bool Add);
