using AccountingManagement.Data;
using AccountingManagement.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore; // Add this

namespace AccountingManagement.Services
{
    public class AuthService
    {
        private readonly AppDbContext _dbContext; // This is now EF Core DbContext
        private User? _currentUser;
        private readonly CustomAuthenticationStateProvider _authStateProvider;

        public AuthService(AppDbContext dbContext, AuthenticationStateProvider authStateProvider)
        {
            System.Diagnostics.Debug.WriteLine("[AuthService] Constructor called.");
            _dbContext = dbContext;
            _authStateProvider = (CustomAuthenticationStateProvider)authStateProvider;
            System.Diagnostics.Debug.WriteLine("[AuthService] Dependencies injected.");
        }

        public User? CurrentUser => _currentUser;

        public async Task<bool> LoginAsync(string username, string password)
        {
            var user = await _dbContext.Users
                                       .AsNoTracking() // Good for read-only queries
                                       .FirstOrDefaultAsync(u => u.Username == username);

            if (user != null && user.IsActive && AppDbContext.VerifyPassword(password, user.PasswordHash))
            {
                _currentUser = user; // Keep a copy for current session state

                // Update LastLoginDate - requires tracking
                var trackedUser = await _dbContext.Users.FindAsync(user.Id);
                if (trackedUser != null)
                {
                    trackedUser.LastLoginDate = DateTime.UtcNow;
                    await _dbContext.SaveChangesAsync();
                }

                _authStateProvider.NotifyUserAuthentication(_currentUser);
                return true;
            }
            return false;
        }

        public void Logout()
        {
            _currentUser = null;
            _authStateProvider.NotifyUserLogout();
        }
    }
}