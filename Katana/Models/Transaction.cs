using System.ComponentModel.DataAnnotations;

namespace Katana.Models;

public class Transaction
{
    public int Id { get; set; }

    public string Note { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:MMMM d}", ApplyFormatInEditMode = true)]
    public DateTime Date { get; set; }

    public List<Entry> Entries { get; set; } = new List<Entry>();
}

