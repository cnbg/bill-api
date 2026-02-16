namespace billing.Constants;

public static class Permissions
{
    public const string ClaimType = "prm";

    // Region
    public const string RegionView = "region-view";
    public const string RegionCreate = "region-create";
    public const string RegionEdit = "region-edit";
    public const string RegionDelete = "region-delete";

    public static List<string> GetList() =>
    [
        // Region
        RegionView, RegionCreate, RegionEdit, RegionDelete,
    ];
}
