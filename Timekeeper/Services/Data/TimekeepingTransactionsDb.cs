using Microsoft.EntityFrameworkCore;
using Timekeeper.Models;

namespace Timekeeper.Services.Data
{
    public class TimekeepingTransactionsDb
    {
        private IDbContextFactory<TimekeeperContext> _contextFactory;

        public TimekeepingTransactionsDb(IDbContextFactory<TimekeeperContext> context)
        {
            _contextFactory = context;
        }

        public async Task<List<TimekeepingTransaction>> GetTimekeepingTransactionsAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var result = await context.TimeKeepingTransactions
                .AsNoTracking()
                .Include(t => t.Employee)
                .Include(t => t.TransactionType)
                .OrderByDescending(t => t.TimeKeepingTransactionId)
                .ToListAsync();

            var list = new List<TimekeepingTransaction>();
            foreach (var t in result)
            {
                list.Add(
                    new TimekeepingTransaction
                    {
                        TimeKeepingTransactionId = t.TimeKeepingTransactionId,
                        TransactionDateTime = t.TransactionDateTime,

                        TransactionTypeId = t.TransactionTypeId,
                        TransactionType = new TransactionType
                        {
                            TransactionTypeId = t.TransactionType.TransactionTypeId,
                            TransactionTypeName = t.TransactionType.TransactionTypeName
                        },

                        EmployeeId = t.EmployeeId,
                        Employee = new Employee
                        {
                            EmployeeId = t.Employee.EmployeeId,
                            FirstName = t.Employee.FirstName,
                            LastName = t.Employee.LastName,
                            Gender = t.Employee.Gender,
                            DateHired = t.Employee.DateHired,
                            Username = t.Employee.Username,
                            Email = t.Employee.Email,
                            AccountType = t.Employee.AccountType
                        }
                    });
            }
            return list;
        }

        public async Task<List<TimekeepingTransaction>> GetTimekeepingTransactionsAsync(int employeeId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var result = await context.TimeKeepingTransactions
                .AsNoTracking()
                .Where(e => e.EmployeeId == employeeId)
                .Include(t => t.Employee)
                .Include(t => t.TransactionType)
                .OrderByDescending(t => t.TimeKeepingTransactionId)
                .ToListAsync();

            var list = new List<TimekeepingTransaction>();
            foreach (var t in result)
            {
                list.Add(
                    new TimekeepingTransaction
                    {
                        TimeKeepingTransactionId = t.TimeKeepingTransactionId,
                        TransactionDateTime = t.TransactionDateTime,

                        TransactionTypeId = t.TransactionTypeId,
                        TransactionType = new TransactionType
                        {
                            TransactionTypeId = t.TransactionType.TransactionTypeId,
                            TransactionTypeName = t.TransactionType.TransactionTypeName
                        },

                        EmployeeId = t.EmployeeId,
                        Employee = new Employee
                        {
                            EmployeeId = t.Employee.EmployeeId,
                            FirstName = t.Employee.FirstName,
                            LastName = t.Employee.LastName,
                            Gender = t.Employee.Gender,
                            DateHired = t.Employee.DateHired,
                            Username = t.Employee.Username,
                            Email = t.Employee.Email,
                            AccountType = t.Employee.AccountType
                        }
                    });
            }
            return list;
        }

        public async Task AddTransactionAsync(TimekeepingTransaction transaction)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            context.TimeKeepingTransactions.Add(transaction);
            await context.SaveChangesAsync();
        }
    }
}
