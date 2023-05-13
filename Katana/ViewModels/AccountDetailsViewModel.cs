using Katana.Models;
using System.ComponentModel.DataAnnotations;

namespace Katana.ViewModels;

public class AccountDetailsViewModel
{
    public class ReportLine
    {
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
    }

    public Account Account { get; set; }

    public List<ReportLine> RegisterReport { get; set; } = new List<ReportLine>();

    public static List<ReportLine> CreateRegisterReport()
    {
        return new List<ReportLine>();
    }
}
