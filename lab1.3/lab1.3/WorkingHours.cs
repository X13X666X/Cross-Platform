using System.Text.Json.Serialization;

namespace ShopCatalog;

public class WorkingHours
{
    [JsonInclude]
    public string Day { get; private set; }

    [JsonInclude]
    public string Hours { get; private set; }

    public WorkingHours()
    {
        Day = "";
        Hours = "";
    }

    public WorkingHours(string day, string hours)
    {
        if (string.IsNullOrWhiteSpace(day))
            throw new ArgumentException("Day cannot be empty.");

        if (string.IsNullOrWhiteSpace(hours))
            throw new ArgumentException("Working hours cannot be empty.");

        Day = day;
        Hours = hours;
    }

    public override string ToString()
    {
        return $"{Day}: {Hours}";
    }
}