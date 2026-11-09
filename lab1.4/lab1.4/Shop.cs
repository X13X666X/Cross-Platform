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

    public Shop(string name, string address, string specialization)
    {
        Validate(name, address, specialization);

        Name = name;
        Address = address;
        Specialization = specialization;

        Phones = Array.Empty<string>();
        Schedule = Array.Empty<WorkingHours>();
    }

    private static void Validate(
        string name,
        string address,
        string specialization)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.");

        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address cannot be empty.");

        if (string.IsNullOrWhiteSpace(specialization))
            throw new ArgumentException("Specialization cannot be empty.");
    }

    public void Update(
        string name,
        string address,
        string specialization)
    {
        Validate(name, address, specialization);

        Name = name;
        Address = address;
        Specialization = specialization;
    }

    public void AddPhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone cannot be empty.");

        string[] newPhones = new string[Phones.Length + 1];

        for (int i = 0; i < Phones.Length; i++)
            newPhones[i] = Phones[i];

        newPhones[Phones.Length] = phone;

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