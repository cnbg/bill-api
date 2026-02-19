namespace billing.Constants;

public static class Permissions
{
    public const string ClaimType = "prm";

    // Region
    public const string RegionView = "region.view";
    public const string RegionCreate = "region.create";
    public const string RegionEdit = "region.edit";
    public const string RegionDelete = "region.delete";

    // Org
    public const string OrgView = "org.view";
    public const string OrgCreate = "org.create";
    public const string OrgEdit = "org.edit";
    public const string OrgDelete = "org.delete";

    public static List<string> GetList() =>
    [
        // Region
        RegionView, RegionCreate, RegionEdit, RegionDelete,

        // Org
        OrgView, OrgCreate, OrgEdit, OrgDelete
    ];
}
