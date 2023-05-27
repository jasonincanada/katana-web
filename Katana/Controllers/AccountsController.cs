using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Katana.Data;
using Katana.Models;
using Katana.Store;
using Katana.ViewModels;

namespace Katana.Controllers
{
    public class AccountsController : Controller
    {
        private readonly KatanaContext _context;
        private readonly BudgetStore _store;

        public AccountsController(KatanaContext context)
        {
            _context = context;
            _store = new BudgetStore(context);
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            Dictionary<Account, decimal> balances = _store.GetAccountBalances();

            var accounts = _context.Accounts
                                   .Include(a => a.BoundTo)
                                   .ToList();

            foreach (var account in accounts)
            {
                if (!balances.ContainsKey(account))
                    balances[account] = 0;
            }

            var vm = accounts.Select(account => new AccountIndexViewModel {
                                 Account = account,
                                 Balance = balances[account]
                              })
                             .OrderBy(a => a.Account.Name)
                             .ToList();
            
            return View(vm);
        }

        // GET: Accounts/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Accounts == null)
                return NotFound();

            var account = _store.GetAccount((int)id);
            if (account == null)
                return NotFound();

            // Get the transactions with at least one entry from this account
            List<Transaction> transactions = _store.GetTransactionsWithAccount(account);
            var report = new List<AccountDetailsViewModel.ReportLine>();
            decimal balance = 0;
            
            foreach (var transaction in transactions)
            foreach (var entry in transaction.Entries.Where(e => e.Account == account))
            {
                balance += entry.Amount;
                report.Add(new AccountDetailsViewModel.ReportLine
                {
                    Date = transaction.Date,
                    Note = transaction.Note,
                    Amount = entry.Amount,
                    Balance = balance
                });
            }

            report.Reverse();

            var vm = new AccountDetailsViewModel
            {
                Account = account,
                RegisterReport = report
            };

            return View(vm);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Accounts == null)
                return NotFound();

            var account = _context.Accounts.Find(id);
            if (account == null)
                return NotFound();

            var envelopes = _context.Envelopes.ToList();

            var viewModel = new AccountEditViewModel
            {
                Account = account,
                SelectedEnvelopeID = account.BoundTo?.Id ?? 0,
                Envelopes = envelopes.OrderBy(e => e.Name)
                                     .Select(envelope => new SelectListItem
                                     {
                                         Value = envelope.Id.ToString(),
                                         Text = envelope.Name
                                     })
            };

            return View(viewModel);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AccountEditViewModel vm)
        {
            if (id != vm.Account.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var account = _store.GetAccount(id);

                    if (account == null)
                        return NotFound();

                    account.Name = vm.Account.Name;

                    if (vm.SelectedEnvelopeID > 0)
                    {
                        var envelope = _context.Envelopes.Find(vm.SelectedEnvelopeID);
                        account.BoundTo = envelope;
                    }
                    else
                    {
                        account.BoundTo = null;
                    }

                    _context.Update(account);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(vm.Account.Id))
                    {
                        return NotFound();
                    }
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Accounts == null)
                return NotFound();

            var account = _context.Accounts
                                  .FirstOrDefault(m => m.Id == id);
            if (account == null)
                return NotFound();

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Accounts == null)
                return Problem("Entity set 'KatanaContext.Account' is null.");

            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
                _context.Accounts.Remove(account);
            
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
          return (_context.Accounts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
