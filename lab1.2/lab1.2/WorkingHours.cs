
namespace ShopCatalog;

public class WorkingHours
{
    private string day;
    private string hours;

    public string Day => day;
    public string Hours => hours;

    public WorkingHours(string day, string hours)
    {
        this.day = day;
        this.hours = hours;
    }

    public override string ToString()
    {
        return $"{day}: {hours}";
    }
}
