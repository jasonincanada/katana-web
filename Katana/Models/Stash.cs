using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Katana.Models;

/// <summary>
/// A Stash is a movement of money from one envelope to another
/// </summary>
public class Stash
{
    [Key]
    public int Id { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:MMMM d}", ApplyFormatInEditMode = true)]
    public DateTime Date { get; set; }

    public int? FromId { get; set; }
    public int? ToId { get; set; }

    [ForeignKey(nameof(FromId))]
    public Envelope? From { get; set; }

    [ForeignKey(nameof(ToId))]
    public Envelope? To { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }
}

// TODO:
// enum StashType
// string Note