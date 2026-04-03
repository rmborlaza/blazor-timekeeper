using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Timekeeper.Components.Pages.Employees;
using Timekeeper.Models;

namespace Timekeeper.Services.Data
{
    public class EmployeeDb
    {
        private IDbContextFactory<TimekeeperContext> _contextFactory;

        public EmployeeDb(IDbContextFactory<TimekeeperContext> context)
        {
            _contextFactory = context;
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var result = await context.Employees.AsNoTracking().OrderBy(e => e.EmployeeId).ToListAsync();

            var list = new List<Employee>();
            foreach (var e in result)
            {
                list.Add(
                    new Employee
                    {
                        EmployeeId = e.EmployeeId,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        Gender = e.Gender,
                        DateHired = e.DateHired,
                        Username = e.Username,
                        Email = e.Email,
                        AccountType = e.AccountType
                    });
            }

            return list;
        }

        public async Task<List<Employee>> GetEmployeesAsync(List<Type> includeTypes)
        {
            throw new NotImplementedException();
        }

        public async Task<Employee> GetEmployeeAsync(int employeeId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var result = await context.Employees.AsNoTracking().Where(e => e.EmployeeId == employeeId).FirstOrDefaultAsync();

            if (result == null)
            {
                return null;
            }

            var employee = new Employee
            {
                EmployeeId = result.EmployeeId,
                FirstName = result.FirstName,
                LastName = result.LastName,
                Gender = result.Gender,
                DateHired = result.DateHired,
                Username = result.Username,
                Email = result.Email,
                PasswordHash = result.PasswordHash,
                AccountType = result.AccountType
            };

            return employee;
        }

        public async Task<Employee> GetEmployeeAsync(int employeeId, List<Type> includeTypes)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var query = context.Employees.AsNoTracking().Where(e => e.EmployeeId == employeeId);

            if (includeTypes.Contains(typeof(TimekeepingTransaction)))
            {
                query = query.Include(e => e.TimeKeepingTransactions).ThenInclude(t => t.TransactionType);
            }

            if (includeTypes.Contains(typeof(Address)))
            {
                query = query.Include(e => e.Address);
            }

            var result = await query.FirstOrDefaultAsync();

            if (result == null)
            {
                return null;
            }

            var employee = new Employee
            {
                EmployeeId = result.EmployeeId,
                FirstName = result.FirstName,
                LastName = result.LastName,
                Gender = result.Gender,
                DateHired = result.DateHired,
                Username = result.Username,
                Email = result.Email,
                PasswordHash = result.PasswordHash,
                AccountType = result.AccountType
            };

            if (includeTypes.Contains(typeof(TimekeepingTransaction)))
            {
                if (employee.TimeKeepingTransactions == null)
                {
                    employee.TimeKeepingTransactions = new List<TimekeepingTransaction>();
                }

                foreach (var transaction in result.TimeKeepingTransactions)
                {
                    employee.TimeKeepingTransactions.Add(new TimekeepingTransaction
                    {
                        TransactionDateTime = transaction.TransactionDateTime,
                        TransactionTypeId = transaction.TransactionTypeId,
                        TransactionType = transaction.TransactionType
                    });
                }
            }

            if (includeTypes.Contains(typeof(Address)))
            {
                employee.Address = new Address
                {
                    StreetAddress = result.Address.StreetAddress,
                    Barangay = result.Address.Barangay,
                    City = result.Address.City
                };
            }

            return employee;
        }

        public async Task<Employee> FindEmployeeAsync(string username, string email, int currentId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var employee = await context.Employees
                .AsNoTracking()
                .Where(e => (e.Username == username || e.Email == email) && e.EmployeeId != currentId)
                .FirstOrDefaultAsync();
            return employee;
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            await context.Employees.AddAsync(employee);
            await context.SaveChangesAsync();
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            if (employee.EmployeeId == 0)
            {
                throw new ArgumentException("EmployeeId cannot be 0.", nameof(employee));
            }

            await using var context = await _contextFactory.CreateDbContextAsync();

            var updateEmployee = await context.Employees
                .Where(e => e.EmployeeId == employee.EmployeeId)
                .Include(e => e.Address)
                .FirstOrDefaultAsync();

            if (updateEmployee != null)
            {
                employee.Address.AddressId = updateEmployee.Address.AddressId;
                employee.Address.EmployeeId = updateEmployee.Address.EmployeeId;

                updateEmployee.FirstName = employee.FirstName;
                updateEmployee.LastName = employee.LastName;
                updateEmployee.Gender = employee.Gender;
                updateEmployee.DateHired = employee.DateHired;

                updateEmployee.Address = employee.Address;

                updateEmployee.Email = employee.Email;
                updateEmployee.Username = employee.Username;
                updateEmployee.AccountType = employee.AccountType;

                context.Employees.Update(updateEmployee);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteEmployeeAsync(Employee employee)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var remove = await GetEmployeeAsync(employee.EmployeeId);
            context.Employees.Remove(remove);
            await context.SaveChangesAsync();

            var unusedTransactionNames = await context.TransactionType
                .Where(tt => !context.TimeKeepingTransactions.Any(tr => tr.TransactionTypeId == tt.TransactionTypeId))
                .ToListAsync();

            if (unusedTransactionNames.Any())
            {
                context.TransactionType.RemoveRange(unusedTransactionNames);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Employee> AuthenticateEmployee(string username, string password)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var employee = await context.Employees
                .AsNoTracking()
                .Where(e =>e.Username == username && e.PasswordHash == Password.CalculateHash(password))
                .FirstOrDefaultAsync();

            return employee;
        }

        public async Task ChangePasswordAsync(int employeeId, string newPassword)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var employee = await context.Employees.FindAsync(employeeId);
            if (employee != null)
            {
                employee.PasswordHash = Password.CalculateHash(newPassword);
                context.Employees.Update(employee);
                await context.SaveChangesAsync();
            }
        }
    }
}
