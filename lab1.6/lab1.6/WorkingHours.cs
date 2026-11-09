namespace ShopCatalog16;

public class WorkingHours
{
    public string Day { get; set; }
    public string Hours { get; set; }

    public WorkingHours()
    {
        Day = "";
        Hours = "";
    }

    public WorkingHours(string day, string hours)
    {
        Day = day;
        Hours = hours;
    }

    public override string ToString()
    {
        return $"{Day}: {Hours}";
    }
}