
using System;
using System.Collections.Generic;

namespace ShopCatalog;

public class Shop
{
    private string name;
    private string address;
    private string specialization;

    private List<string> phones;
    private List<WorkingHours> schedule;

    public string Name => name;
    public string Address => address;
    public string Specialization => specialization;

    public Shop(string name, string address, string specialization)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.");

        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address cannot be empty.");

        if (string.IsNullOrWhiteSpace(specialization))
            throw new ArgumentException("Specialization cannot be empty.");

        this.name = name;
        this.address = address;
        this.specialization = specialization;

        phones = new List<string>();
        schedule = new List<WorkingHours>();
    }

    public void AddPhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone cannot be empty.");

        phones.Add(phone);
    }

    public void AddWorkingHours(string day, string hours)
    {
        if (string.IsNullOrWhiteSpace(day))
            throw new ArgumentException("Day cannot be empty.");

        if (string.IsNullOrWhiteSpace(hours))
            throw new ArgumentException("Working hours cannot be empty.");

        schedule.Add(new WorkingHours(day, hours));
    }

    public override string ToString()
    {
        string phoneInfo = phones.Count > 0
            ? string.Join(", ", phones)
            : "Not specified";

        string scheduleInfo = schedule.Count > 0
            ? string.Join("\n    ", schedule)
            : "Not specified";

        return
            $"Name: {name}\n" +
            $"Address: {address}\n" +
            $"Phones: {phoneInfo}\n" +
            $"Specialization: {specialization}\n" +
            $"Working schedule:\n    {scheduleInfo}";
    }
}
