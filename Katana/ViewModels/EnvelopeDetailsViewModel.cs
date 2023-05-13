using Katana.Models;
using System.ComponentModel.DataAnnotations;

namespace Katana.ViewModels;

public class EnvelopeDetailsViewModel
{
    public Envelope Envelope { get; set; }

    public List<Account> BoundAccounts { get; set; } = new();
        
    public List<Transaction> Transactions { get; set; } = new();

    public List<Stash> Stashes { get; set; } = new();

    [DisplayFormat(DataFormatString = "{0:F2}")]
    public decimal FundsInEnvelope { get; set; }

    [DisplayFormat(DataFormatString = "{0:N2}")]
    public decimal NetStashed { get; set; }

    [DisplayFormat(DataFormatString = "{0:N2}")]
    public decimal SpendingTotal { get; set; }


    public bool IsAvailableEnvelope { get; set; }

    #region Available envelope only

    [DisplayFormat(DataFormatString = "{0:N2}")]
    public decimal TotalInflow { get; set; }

    [DisplayFormat(DataFormatString = "{0:N2}")]
    public decimal AvailableFunds { get; set; }

    public bool IsUnfundedSpending { get; set; }

    public List<EntryWithDateAndNote> InflowEntries { get; set; } = new();

    #endregion
}

public class EntryWithDateAndNote
{
    [DisplayFormat(DataFormatString = "{0:MMMM d}")]
    public DateTime Date { get; set; }
    public string Note { get; set; }
    public Entry Entry { get; set; }
}