using AccountingManagement.Application.Users.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AccountingManagement.UI.Maui.Authentication;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly CurrentUserService _currentUserService;

    public CustomAuthenticationStateProvider(CurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(_currentUserService.CurrentUserPrincipal));
    }

    public void NotifyUserAuthentication(UserDto user)
    {
        _currentUserService.SetCurrentUser(user);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void NotifyUserLogout()
    {
        _currentUserService.ClearCurrentUser();
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}