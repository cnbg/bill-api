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

    // Role
    public const string RoleView = "role.view";
    public const string RoleCreate = "role.create";
    public const string RoleEdit = "role.edit";
    public const string RoleDelete = "role.delete";

    // ClientType
    public const string ClientTypeView = "client-type.view";
    public const string ClientTypeCreate = "client-type.create";
    public const string ClientTypeEdit = "client-type.edit";
    public const string ClientTypeDelete = "client-type.delete";

    // Client
    public const string ClientView = "client.view";
    public const string ClientCreate = "client.create";
    public const string ClientEdit = "client.edit";
    public const string ClientDelete = "client.delete";

    // Rate
    public const string RateView = "rate.view";
    public const string RateCreate = "rate.create";
    public const string RateEdit = "rate.edit";
    public const string RateDelete = "rate.delete";

    // Payment
    public const string PaymentView = "payment.view";
    public const string PaymentCreate = "payment.create";
    public const string PaymentEdit = "payment.edit";
    public const string PaymentDelete = "payment.delete";

    // Charge
    public const string ChargeView = "charge.view";
    public const string ChargeCreate = "charge.create";
    public const string ChargeEdit = "charge.edit";
    public const string ChargeDelete = "charge.delete";

    // Expense
    public const string ExpenseView = "expense.view";
    public const string ExpenseCreate = "expense.create";
    public const string ExpenseEdit = "expense.edit";
    public const string ExpenseDelete = "expense.delete";

    // Balance
    public const string BalanceView = "balance.view";
    public const string BalanceCreate = "balance.create";
    public const string BalanceEdit = "balance.edit";
    public const string BalanceDelete = "balance.delete";

    // Article
    public const string ArticleView = "article.view";
    public const string ArticleCreate = "article.create";
    public const string ArticleEdit = "article.edit";
    public const string ArticleDelete = "article.delete";

    // User
    public const string UserView = "user.view";
    public const string UserCreate = "user.create";
    public const string UserEdit = "user.edit";
    public const string UserDelete = "user.delete";

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

        // Role
        RoleView, RoleCreate, RoleEdit, RoleDelete,

        // ClientType
        ClientTypeView, ClientTypeCreate, ClientTypeEdit, ClientTypeDelete,

        // Client
        ClientView, ClientCreate, ClientEdit, ClientDelete,

        // Payment
        PaymentView, PaymentCreate, PaymentEdit, PaymentDelete,

        // Charge
        ChargeView, ChargeCreate, ChargeEdit, ChargeDelete,

        // Balance
        BalanceView, BalanceCreate, BalanceEdit, BalanceDelete,

        // Expense
        ExpenseView, ExpenseCreate, ExpenseEdit, ExpenseDelete,

        // Article
        ArticleView, ArticleCreate, ArticleEdit, ArticleDelete,

        // User
        UserView, UserCreate, UserEdit, UserDelete
    ];
}
