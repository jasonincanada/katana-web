using Katana.Models;
using System.ComponentModel.DataAnnotations;

namespace Katana.ViewModels;

public class AccountIndexViewModel
{
    public Account Account { get; set; }

    [DisplayFormat(DataFormatString="{0:N2}")]
    public decimal Balance { get; set; }
}
