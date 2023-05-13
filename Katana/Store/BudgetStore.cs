using Katana.Data;
using Katana.Models;
using Microsoft.EntityFrameworkCore;

namespace Katana.Store
{
    public interface IBudgetStore
    {
        /* Accounts */

        Account? GetAccount(int id);
        List<Account> GetBoundAccounts(Envelope envelope);


        /* Transactions */

        public Transaction? GetTransaction(int id);

        // Get all transactions that have at least one entry involving this account
        List<Transaction> GetTransactionsWithAccount(Account account);


        /* Envelopes */

        Envelope? GetEnvelope(int id);

        void Stash(Stash stash);


        /* Reports */

        Dictionary<Account, decimal> GetAccountBalances();
        decimal GetAvailable();
        decimal GetSpendingTotal(Envelope envelope);
    }


    public class BudgetStore : IBudgetStore
    {
        private readonly KatanaContext _context;

        public BudgetStore(KatanaContext context) => _context = context;

        #region Accounts

        public Account? GetAccount(int id)
        {
            return _context
                .Accounts
                .Include(a => a.BoundTo)
                .FirstOrDefault(a => a.Id == id);
        }

        /// <summary>
        /// Get accounts bound to this envelope
        /// </summary>
        /// <param name="envelope"></param>
        /// <returns></returns>
        public List<Account> GetBoundAccounts(Envelope envelope)
        {
            return _context.Accounts
                           .Where(a => a.BoundTo == envelope)
                           .ToList();
        }

        #endregion
        #region Transactions

        /// <summary>
        /// Get an individual transaction, including its entries and bound accounts
        /// </summary>
        /// <param name="id">The Transaction.Id for the transaction</param>
        public Transaction? GetTransaction(int id)
        {
            return _context.Transactions
                           .Include(t => t.Entries)
                           .ThenInclude(e => e.Account)
                           .Where(t => t.Id == id)
                           .FirstOrDefault();
        }

        /// <summary>
        /// Get a list of transactions with at least one entry from this account
        /// </summary>
        public List<Transaction> GetTransactionsWithAccount(Account account)
        {
            return _context.Transactions
                           .Include(trans => trans.Entries)
                           .ThenInclude(entry => entry.Account)
                           .Where(t => t.Entries.Any(e => e.Account == account))
                           .OrderBy(t => t.Date)
                           .ToList();
        }

        #endregion
        #region Envelopes

        public Envelope? GetEnvelope(int id) => _context.Envelopes.FirstOrDefault(e => e.Id == id);

        #endregion
        #region Reports

        public Dictionary<Account, decimal> GetAccountBalances()
        {
            var accountBalances = _context.Transactions
                .SelectMany(t => t.Entries)
                .GroupBy(entry => entry.Account.Id)
                .ToDictionary(group => group.Key,
                              group => group.Sum(entry => entry.Amount));

            var accounts = _context.Accounts
                .ToDictionary(account => account.Id);

            return accountBalances
                .ToDictionary(kvp => accounts[kvp.Key],
                              kvp => kvp.Value);
        }

        /// <summary>
        /// Move an amount from one Envelope to another, updating the .Amount field for each accordingly
        /// </summary>
        public void Stash(Stash stash)
        {
            _context.Stashes.Add(stash);

            var from = _context.Envelopes.Find(stash.From.Id);
            var to   = _context.Envelopes.Find(stash.To.Id);

            // TODO: can we do this??
            from.Amount -= stash.Amount;
            to.Amount += stash.Amount;
        }

        /// <summary>
        /// Get the amount of funds sitting in the Available envelope. This is the sum of positive inflows
        /// to accounts starting with "assets" plus the net amount stashed into the Available envelope
        /// </summary>
        /// <returns></returns>
        public decimal GetAvailable()
        {
            // total up the inflow for the Available envelope
            decimal inflow = _context
                .Transactions
                .Include(t => t.Entries)
                .ThenInclude(e => e.Account)
                .SelectMany(t => t.Entries)
                .Where(e => e.Account.Name.StartsWith("assets")

                            /* This isn't correct, we need to see the other side of this
                             * transaction but that info is gone by this point. And sometimes
                             * can't even be gotten anyway ("split source" transactions?)
                             * 
                             * && e.Entry.Account.BoundTo == null */)

                .Where(e => e.Amount > 0)
                .Sum(e => e.Amount);

            decimal netStashed =
                _context.Stashes
                        //.Include(s => s.From)
                        //.Include(s => s.To)
                        .Where(stash => stash.To.Id   == SpecialEnvelope.Available
                                     || stash.From.Id == SpecialEnvelope.Available)
                        .Sum(stash => (stash.To.Id == SpecialEnvelope.Available ? 1 : -1) * stash.Amount);

            return inflow + netStashed;
        }


        /// <summary>
        /// Return the sum of positive amounts of all entries bound to this account
        /// </summary>
        public decimal GetSpendingTotal(Envelope envelope)
        {
            return _context.Transactions
                           .Include(t => t.Entries)
                           .ThenInclude(e => e.Account)
                           .SelectMany(t => t.Entries)
                           .Where(entry => entry.Account.BoundTo == envelope)
                           .Where(entry => entry.Amount > 0)
                           .Sum(entry => entry.Amount);
        }

        #endregion
    }
}
