using System;
using System.Text.Json.Serialization;

namespace ShopCatalog14;

public class Shop
{
    [JsonInclude]
    public string Name { get; private set; }

    [JsonInclude]
    public string Address { get; private set; }

    [JsonInclude]
    public string Specialization { get; private set; }

    [JsonInclude]
    public string[] Phones { get; private set; }

    [JsonInclude]
    public WorkingHours[] Schedule { get; private set; }

    public Shop()
    {
        Name = "";
        Address = "";
        Specialization = "";

        Phones = Array.Empty<string>();
        Schedule = Array.Empty<WorkingHours>();
    }

    public Shop(
        string name,
        string address,
        string specialization)
    {
        Validate(name, address, specialization);

        Name = name.Trim();
        Address = address.Trim();
        Specialization = specialization.Trim();

        Phones = Array.Empty<string>();
        Schedule = Array.Empty<WorkingHours>();
    }

    private static void Validate(
        string name,
        string address,
        string specialization)
    {
        if (!RegexValidator.IsValidName(name))
            throw new ArgumentException("Invalid shop name.");

        if (!RegexValidator.IsValidAddress(address))
            throw new ArgumentException("Invalid shop address.");

        if (!RegexValidator.IsValidName(specialization))
            throw new ArgumentException("Invalid specialization.");
    }

    public void Update(
        string name,
        string address,
        string specialization)
    {
        Validate(name, address, specialization);

        Name = name.Trim();
        Address = address.Trim();
        Specialization = specialization.Trim();
    }

    public void AddPhone(string phone)
    {
        if (!RegexValidator.IsValidPhone(phone))
            throw new ArgumentException("Invalid mobile phone number.");

        string normalized = RegexValidator.NormalizePhone(phone);

        string[] newPhones = new string[Phones.Length + 1];

        for (int i = 0; i < Phones.Length; i++)
            newPhones[i] = Phones[i];

        newPhones[Phones.Length] = normalized;

        Phones = newPhones;
    }

    public void RemovePhone(int index)
    {
        if (index < 0 || index >= Phones.Length)
            throw new ArgumentOutOfRangeException(nameof(index));

        string[] newPhones = new string[Phones.Length - 1];

        int newIndex = 0;

        for (int i = 0; i < Phones.Length; i++)
        {
            if (i != index)
            {
                newPhones[newIndex] = Phones[i];
                newIndex++;
            }
        }

        Phones = newPhones;
    }

    public void SetSchedule(string day, string hours)
    {
        if (!RegexValidator.IsValidScheduleEntry(hours))
            throw new ArgumentException("Invalid working schedule.");

        WorkingHours newHours = new WorkingHours(day, hours);

        for (int i = 0; i < Schedule.Length; i++)
        {
            if (string.Equals(
                Schedule[i].Day,
                day,
                StringComparison.OrdinalIgnoreCase))
            {
                Schedule[i] = newHours;
                return;
            }
        }

        WorkingHours[] newSchedule =
            new WorkingHours[Schedule.Length + 1];

        for (int i = 0; i < Schedule.Length; i++)
            newSchedule[i] = Schedule[i];

        newSchedule[Schedule.Length] = newHours;

        Schedule = newSchedule;
    }

    public override string ToString()
    {
        string phoneInfo = Phones.Length > 0
            ? string.Join(", ", Phones)
            : "Not specified";

        string scheduleInfo = "";

        if (Schedule.Length == 0)
        {
            scheduleInfo = "Not specified";
        }
        else
        {
            for (int i = 0; i < Schedule.Length; i++)
            {
                scheduleInfo += Schedule[i];

                if (i < Schedule.Length - 1)
                    scheduleInfo += "\n    ";
            }
        }

        return
            $"Name: {Name}\n" +
            $"Address: {Address}\n" +
            $"Phones: {phoneInfo}\n" +
            $"Specialization: {Specialization}\n" +
            $"Working schedule:\n    {scheduleInfo}";
    }
}