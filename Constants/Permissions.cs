namespace billing.Constants;

public static class Permissions
{
    public const string ClaimType = "prm";

    // Region
    public const string RegionView = "region.view";
    public const string RegionCreate = "region.create";
    public const string RegionEdit = "region.edit";
    public const string RegionDelete = "region.delete";

    // District
    public const string DistrictView = "district.view";
    public const string DistrictCreate = "district.create";
    public const string DistrictEdit = "district.edit";
    public const string DistrictDelete = "district.delete";

    // OrgType
    public const string OrgTypeView = "org-type.view";
    public const string OrgTypeCreate = "org-type.create";
    public const string OrgTypeEdit = "org-type.edit";
    public const string OrgTypeDelete = "org-type.delete";

    // Org
    public const string OrgView = "org.view";
    public const string OrgCreate = "org.create";
    public const string OrgEdit = "org.edit";
    public const string OrgDelete = "org.delete";

    // Payment
    public const string PaymentView = "payment.view";
    public const string PaymentCreate = "payment.create";
    public const string PaymentEdit = "payment.edit";
    public const string PaymentDelete = "payment.delete";

    public static List<string> GetList() =>
    [
        // Region
        RegionView, RegionCreate, RegionEdit, RegionDelete,

        // District
        DistrictView, DistrictCreate, DistrictEdit, DistrictDelete,

        // OrgType
        OrgTypeView, OrgTypeCreate, OrgTypeEdit, OrgTypeDelete,

        // Org
        OrgView, OrgCreate, OrgEdit, OrgDelete,

        // Payment
        PaymentView, PaymentCreate, PaymentEdit, PaymentDelete
    ];
}
