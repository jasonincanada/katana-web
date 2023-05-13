using Microsoft.EntityFrameworkCore;
using Katana.Models;

namespace Katana.Data
{
    public class KatanaContext : DbContext
    {
        public KatanaContext (DbContextOptions<KatanaContext> options)
            : base(options)
        {
        }

        public DbSet<Envelope> Envelopes { get; set; } = default!;
        public DbSet<Account> Accounts { get; set; } = default!;
        public DbSet<Entry> Entries { get; set; } = default!;
        public DbSet<Transaction> Transactions { get; set; } = default!;
        public DbSet<Stash> Stashes { get; set; } = default!;
    }
}
