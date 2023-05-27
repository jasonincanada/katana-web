using Katana.Models;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Katana.TagHelpers
{
    // Code for our custom <katana> tag, which standardizes various budgeting components everywhere
    // they appear in the UI, usually adding links to go to the relevant detail page
    //
    // Usages:
    //
    //     The account is <katana account="Model.Account"></katana>
    //     It is bound to the <katana envelope="Model.Account.BoundTo"></katana> envelope
    //     The current balance is <katana amount="Model.Account.Amount"></katana>
    //
    [HtmlTargetElement("katana")]
    public class KatanaTagHelper : TagHelper
    {
        public Account Account { get; set; }
        public Envelope Envelope { get; set; }

        public bool HighlightNegatives { get; set; }

        public decimal? Amount { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
                 if (Account  != null) output.A($"/Accounts/Details/{Account.Id}", "account", Account.Name);
            else if (Envelope != null) output.A($"/Envelopes/Details/{Envelope.Id}", "envelope", Envelope.Name);
            else if (Amount != null)
            {
                output.TagName = "span";
                output.AddClass("amount");

                if (Amount.Value < 0 && HighlightNegatives)
                    output.AddClass("negative-amount");

                if (Amount.Value == 0)
                {
                    output.AddClass("zero-amount");
                    output.Content.SetContent("--");
                }
                else
                {
                    string s = string.Format("{0:N2}", (decimal)Amount.Value);

                    string[] split = s.Split('.');
                    string dollars = split[0];
                    string cents = split[1];

                    string final =
                          $"<span class=\"segment-dollars\">{dollars}</span>"
                        + $"<span class=\"segment-dot\"></span>"
                        + $"<span class=\"segment-cents\">.{cents}</span>";

                    output.Content.SetHtmlContent(final);
                }
            }
        }
    }

    public static class TagHelperOutputExtensions
    {
        public static void A(this TagHelperOutput output, string href, string @class, string content)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("href", href);
            AddClass(output, @class);
            output.Content.SetContent(content);
        }

        public static void AddClass(this TagHelperOutput output, string @class)
        {
            var existingClass = output.Attributes.FirstOrDefault(a => a.Name == "class");
            if (existingClass != null)
            {
                var newClassValue = $"{existingClass.Value} {@class}";
                output.Attributes.SetAttribute("class", newClassValue);
            }
            else
            {
                output.Attributes.SetAttribute("class", @class);
            }
        }
    }
}
