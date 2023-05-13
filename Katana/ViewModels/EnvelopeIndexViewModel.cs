using Katana.Models;

namespace Katana.ViewModels;

public class EnvelopeIndexViewModel
{
    public Envelope Envelope { get; set; }
    public bool IsAvailableEnvelope { get; set; }

    public decimal TotalBoundSpending { get; set; }

    public decimal AmountInEnvelope { get; set; }
}
