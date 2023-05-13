using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Katana.Models;

/// <summary>
/// A transaction entry
/// </summary>
public class Entry
{
    public int Id { get; set; }

    public int TransactionId { get; set; }
    public Transaction Transaction { get; set; }

    public Account Account { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:N2}")]
    public decimal Amount { get; set; }
}
