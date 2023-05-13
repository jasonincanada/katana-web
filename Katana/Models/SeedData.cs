using Katana.Data;
using Microsoft.EntityFrameworkCore;

namespace Katana.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new KatanaContext(serviceProvider.GetRequiredService<DbContextOptions<KatanaContext>>());

            // Bail if we have existing data
            if (context.Envelopes.Any() || context.Accounts.Any())
                return;

            // Built-in envelopes
            context.Envelopes.Add(new Envelope { Name = "✉️ Available" });
            context.Envelopes.Add(new Envelope { Name = "🍞 Groceries" });

            // Built-in accounts
            //context.Accounts.Add(Account.New("assets:cash"));
            //context.Accounts.Add(Account.New("assets:savings"));
            //context.Accounts.Add(Account.New("credit:visa"));

            context.SaveChanges();
        }
    }
}
