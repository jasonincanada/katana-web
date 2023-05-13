using Katana.Models;
using System.ComponentModel.DataAnnotations;

namespace Katana.ViewModels;

public class StashViewModel
{
    /// <summary>
    /// The stashing session starts off with this much Available
    /// </summary>
    public decimal Available { get; set; }

    public List<StashRow> Rows { get; set; } = new();
}

public class StashRow
{
    public Envelope Envelope { get; set; }

    /// <summary>
    /// The net amount stashed from Available to this envelope in this session
    /// </summary>
    public decimal Stashing { get; set; }

    [DisplayFormat(DataFormatString = "{0:N2}")]
    public decimal NewAmount { get; set; }

    public override string ToString()
    {
        return $"{Envelope.Id} {Envelope.Name}: {Stashing}";
    }
}
