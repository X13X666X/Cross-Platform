namespace ShopCatalog14;

public class ShopSearchCriteria
{
    public bool ExcludeFoodStores { get; set; } = true;

    public bool RequireAllDays { get; set; } = true;

    public bool RequireNoLunchBreak { get; set; } = true;

    public bool RequireUkrainianMobilePhone { get; set; } = true;
}