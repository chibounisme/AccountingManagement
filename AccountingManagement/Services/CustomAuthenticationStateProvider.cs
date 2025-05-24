using AccountingManagement.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json; // For potential session persistence (not fully implemented here)

namespace AccountingManagement.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        // In a desktop app, session persistence can be tricky without web cookies.
        // SecureStorage can be used for tokens, or simply rely on in-memory state for the session.
        private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(_currentUser));
        }

        public void NotifyUserAuthentication(User user)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.GivenName, user.FullName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }, "Auth"); // "Auth" is the authentication type

            _currentUser = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void NotifyUserLogout()
        {
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity()); // Anonymous user
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}