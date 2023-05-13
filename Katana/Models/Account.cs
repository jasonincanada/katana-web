using System.ComponentModel.DataAnnotations;

namespace Katana.Models;

public class Account
{
    public static Account New(string accountName)
    {
        return new Account
        {
            Name = accountName
        };
    }

    public int Id { get; set; }
    public string Name { get; set; }

    [Display(Name="Bound To Envelope")]
    public Envelope? BoundTo { get; set; }

    public override string ToString()
    {
        if (BoundTo == null)
            return $"{Name}";
        else
            return $"{Name} (${BoundTo.Name})";
    }
}
