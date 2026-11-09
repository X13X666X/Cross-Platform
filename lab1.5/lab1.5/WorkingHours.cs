using System;
using System.Text.Json.Serialization;

namespace ShopCatalog14;

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

        if (!RegexValidator.IsValidScheduleEntry(hours))
            throw new ArgumentException("Invalid working schedule.");

        Day = day.Trim();
        Hours = hours.Trim();
    }

    public override string ToString()
    {
        return $"{Day}: {Hours}";
    }
}