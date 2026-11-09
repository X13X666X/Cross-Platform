using System;
using System.Collections.Generic;

namespace ShopCatalog16;

public class Shop
{
    public string Name { get; set; }
    public string Address { get; set; }
    public List<string> Phones { get; set; }
    public string Specialization { get; set; }
    public List<WorkingHours> Schedule { get; set; }

    public Shop()
    {
        Name = "";
        Address = "";
        Specialization = "";

        Phones = new List<string>();
        Schedule = new List<WorkingHours>();
    }

    public Shop(
        string name,
        string address,
        string specialization)
    {
        Name = name;
        Address = address;
        Specialization = specialization;

        Phones = new List<string>();
        Schedule = new List<WorkingHours>();
    }

    public void AddPhone(string phone)
    {
        if (!string.IsNullOrWhiteSpace(phone))
        {
            Phones.Add(phone.Trim());
        }
    }

    public void RemovePhone(int index)
    {
        if (index >= 0 && index < Phones.Count)
        {
            Phones.RemoveAt(index);
        }
    }

    public void AddWorkingHours(string day, string hours)
    {
        WorkingHours existing = Schedule.Find(
            item => item.Day.Equals(
                day,
                StringComparison.OrdinalIgnoreCase));

        if (existing != null)
        {
            existing.Hours = hours;
        }
        else
        {
            Schedule.Add(new WorkingHours(day, hours));
        }
    }

    public override string ToString()
    {
        string phones = Phones.Count > 0
            ? string.Join(", ", Phones)
            : "No phones";

        string schedule = Schedule.Count > 0
            ? string.Join("\n    ", Schedule)
            : "Not specified";

        return
            $"Name: {Name}\n" +
            $"Address: {Address}\n" +
            $"Phones: {phones}\n" +
            $"Specialization: {Specialization}\n" +
            $"Working schedule:\n    {schedule}";
    }
}