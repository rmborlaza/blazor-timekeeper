using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Timekeeper.Models;

namespace Timekeeper.Services.AuthenticationService
{
    public class Authentication
    {
        private IHttpContextAccessor _contextAccessor;
        private IDbContextFactory<TimekeeperContext> _context;

        public string? CurrentUser => _contextAccessor.HttpContext?.User.Identity?.Name;

        public Authentication(IHttpContextAccessor contextAccessor, IDbContextFactory<TimekeeperContext> context)
        {
            _contextAccessor = contextAccessor;
            _context = context;
        }

        public async Task<Employee> LoginAsync(string username, string password)
        {
            await using var context = await _context.CreateDbContextAsync();

            var employee = await context.Employees
                .AsNoTracking()
                .Where(e => e.Username == username && e.PasswordHash == Password.CalculateHash(password))
                .FirstOrDefaultAsync();
            if (employee == null)
            {
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim("EmployeeId", employee.EmployeeId.ToString()),
                new Claim(ClaimTypes.Name, employee.FirstName),
                new Claim(ClaimTypes.Surname, employee.LastName),
                new Claim(ClaimTypes.Role, employee.AccountType.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await _contextAccessor.HttpContext!.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return employee;
        }

        public async Task LogoutAsync()
        {
            await _contextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
