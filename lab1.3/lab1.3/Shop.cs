using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ShopCatalog;

public class Shop
{
    [JsonInclude]
    public string Name { get; private set; }

    [JsonInclude]
    public string Address { get; private set; }

    [JsonInclude]
    public string Specialization { get; private set; }

    [JsonInclude]
    public List<string> Phones { get; private set; }

    [JsonInclude]
    public List<WorkingHours> Schedule { get; private set; }

    public Shop()
    {
        Name = "";
        Address = "";
        Specialization = "";
        Phones = new List<string>();
        Schedule = new List<WorkingHours>();
    }

    public Shop(string name, string address, string specialization)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.");

        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address cannot be empty.");

        if (string.IsNullOrWhiteSpace(specialization))
            throw new ArgumentException("Specialization cannot be empty.");

        Name = name;
        Address = address;
        Specialization = specialization;

        Phones = new List<string>();
        Schedule = new List<WorkingHours>();
    }

    public void Update(string name, string address, string specialization)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.");

        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address cannot be empty.");

        if (string.IsNullOrWhiteSpace(specialization))
            throw new ArgumentException("Specialization cannot be empty.");

        Name = name;
        Address = address;
        Specialization = specialization;
    }

    public void AddPhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone cannot be empty.");

        Phones.Add(phone);
    }

    public void RemovePhone(int index)
    {
        if (index < 0 || index >= Phones.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        Phones.RemoveAt(index);
    }

    public void SetSchedule(string day, string hours)
    {
        if (string.IsNullOrWhiteSpace(day))
            throw new ArgumentException("Day cannot be empty.");

        if (string.IsNullOrWhiteSpace(hours))
            throw new ArgumentException("Working hours cannot be empty.");

        int index = Schedule.FindIndex(
            item => item.Day.Equals(day, StringComparison.OrdinalIgnoreCase));

        WorkingHours newHours = new WorkingHours(day, hours);

        if (index >= 0)
            Schedule[index] = newHours;
        else
            Schedule.Add(newHours);
    }

    public override string ToString()
    {
        string phoneInfo = Phones.Count > 0
            ? string.Join(", ", Phones)
            : "Not specified";

        string scheduleInfo = Schedule.Count > 0
            ? string.Join("\n    ", Schedule)
            : "Not specified";

        return
            $"Name: {Name}\n" +
            $"Address: {Address}\n" +
            $"Phones: {phoneInfo}\n" +
            $"Specialization: {Specialization}\n" +
            $"Working schedule:\n    {scheduleInfo}";
    }
}