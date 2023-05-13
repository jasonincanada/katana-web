using Katana.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Katana.ViewModels;

public class AccountEditViewModel
{
    public Account Account { get; set; }

    public IEnumerable<SelectListItem> Envelopes { get; set; } = new List<SelectListItem>();

    [Display(Name="Bind to Envelope")]
    public int SelectedEnvelopeID { get; set; }
}
