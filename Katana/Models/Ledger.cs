using System.Text.RegularExpressions;

namespace Katana.Models;

public class ParseException : Exception { }

public class Ledger
{
    public List<LedgerTransaction> LedgerTransactions { get; private set; }

    public static Ledger ParseJournal(string lines)
    {
        return new Ledger
        {
            LedgerTransactions = 
                lines.Split(new string[] { "\n\n", "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                     .Select(LedgerTransaction.ParseFromLedger)
                     .Where(x => x != null)
                     .ToList()
        };
    }

    public static string RemoveComment(string l)
    {
        string[] split = l.Split(new char[] { ';' });

        if (split.Length == 1)
            return l;

        return split[0].TrimEnd();
    }

    public static void Balance(LedgerTransaction transaction)
    {
        if (transaction.Entries.All(e => e.Amount != null))
        {
            if (transaction.Entries.Select(e => e.Amount).Sum() != 0)
                throw new UnbalancedTransactionException();

            return;
        }

        if (transaction.Entries.Count(e => e.Amount == null) > 1)
            throw new MoreThanOneBlankAmountException();

        var blank = transaction.Entries.First(e => e.Amount == null);

        // Negative of the sum of the other entries
        blank.Amount = -transaction.Entries
                                   .Where(e => e.Amount != null)
                                   .Sum(e => e.Amount);
    }

    public override string ToString()
    {
        return $"{LedgerTransactions.Count} transactions";
    }
}

//
// ;note
// 2023/03/15 Sandwich
//    assets:savings                     $-6.76 ; note
//    assets:cash                           $-1
//    expenses:tips                          $1
//    expenses:food:tim-hortons
//

public class LedgerTransaction
{
    public DateTime Date { get; set; }
    public string Note { get; set; }

    public List<LedgerEntry> Entries { get; set; }

    public static LedgerTransaction? ParseFromLedger(string chunk)
    {
        List<string> split = chunk.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                                  .Select(Ledger.RemoveComment)
                                  .Where(l => !string.IsNullOrEmpty(l))
                                  .ToList();

        if (split.Count == 0)
            return null;

        var header = split[0];

        if (DateTime.TryParse(header[..10], out var date))
        {
            return new LedgerTransaction
            {
                Date = date,
                Note = header[10..].Trim(),
                Entries = split.Skip(1)
                               .Select(LedgerEntry.ParseFromLedgerLine)
                               .Where(x => x != null)
                               .ToList()
            };
        }
        else
            throw new ParseException();
    }

    public override string ToString()
    {
        return $"{Date} <{Note}> {Entries.Count} entries";
    }
}

public class LedgerEntry
{
    public string AccountName { get; set; }
    public decimal? Amount { get; set; }

    public static LedgerEntry? ParseFromLedgerLine(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            throw new ArgumentNullException(nameof(line), "null or empty ledger line");
        
        line = Ledger.RemoveComment(line);

        // Split the input into parts using whitespace
        var parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        // we don't handle things like $ 3.62 or 308 kWh yet
        if (parts.Length > 2)
            return null;

        var accountName = parts[0].Trim();

        if (parts.Length > 1)
        {
            var amount = parts[1];

            if (!amount.StartsWith("$"))
                return null;

            // Remove any currency symbols and parse the amount
            amount = amount.TrimStart('$');

            if (!decimal.TryParse(amount, out decimal parsedAmount))
                throw new ArgumentException("The provided amount is not a valid decimal value.");

            return new LedgerEntry
            {
                AccountName = accountName,
                Amount = parsedAmount
            };
        }
        else
        {
            return new LedgerEntry
            {
                AccountName = accountName,
                Amount = null
            };
        }
    }

    public override string ToString()
    {
        if (Amount == null)
            return $"{AccountName} (blank)";
        else
            return $"{AccountName} {Amount}";
    }
}


public class UnbalancedTransactionException : Exception { }
public class MoreThanOneBlankAmountException : Exception { }
