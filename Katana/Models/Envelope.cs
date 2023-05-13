using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Katana.Models;

public class Envelope
{
    public int Id { get; set; }
    public string Name { get; set; } = "New Envelope";

    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:N2}")]
    public decimal Amount { get; set; }

    [Display(Name="Envelope Color")]
    [StringLength(7, ErrorMessage = "The {0} must be exactly {1} characters long.", MinimumLength = 7)]
    [RegularExpression(@"^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "Invalid hex color code")]
    public string? HexColor { get; set; }

    public override string ToString()
    {
        return $"{Id} {Name}";
    }

    public static Envelope Available()
    {
        return new Envelope
        {
            Id = 1,
            Name = "✉️ Available"
        };
    }
}
