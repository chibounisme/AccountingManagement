using System;
using System.Collections.Generic;
using AccountingManagement.Application.Abstractions;
using AccountingManagement.Application.Users.DTOs;
using AccountingManagement.Domain.Enums;
using System.Security.Claims;

namespace AccountingManagement.UI.Maui.Authentication;

public class CurrentUserService : ICurrentUserService
{
    private ClaimsPrincipal _currentUserPrincipal = new(new ClaimsIdentity());

    public ClaimsPrincipal CurrentUserPrincipal => _currentUserPrincipal;

    // ICurrentUserService implementation
    public int? UserId
    {
        get
        {
            if (_currentUserPrincipal?.Identity?.IsAuthenticated != true)
            {
                return null;
            }
            // Use FindFirst and then access .Value
            Claim? nameIdentifierClaim = _currentUserPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            return int.TryParse(nameIdentifierClaim?.Value, out var id) ? id : null;
        }
    }

    public string? Username
    {
        get
        {
            if (_currentUserPrincipal?.Identity?.IsAuthenticated != true)
            {
                return null;
            }
            // Use FindFirst and then access .Value
            return _currentUserPrincipal.FindFirst(ClaimTypes.Name)?.Value;
        }
    }

    public UserRole? UserRole
    {
        get
        {
            if (_currentUserPrincipal?.Identity?.IsAuthenticated != true)
            {
                return null;
            }
            
            // Use FindFirst and then access .Value
            string? roleString = _currentUserPrincipal.FindFirst(ClaimTypes.Role)?.Value;
            return Enum.TryParse<UserRole>(roleString, out var role) ? role : null;
        }
    }

    public bool IsAuthenticated => _currentUserPrincipal?.Identity?.IsAuthenticated ?? false;


    // Methods for CustomAuthenticationStateProvider to use
    public void SetCurrentUser(UserDto user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        if (!string.IsNullOrEmpty(user.FullName))
        {
            claims.Add(new Claim(ClaimTypes.GivenName, user.FullName));
        }
        if (!string.IsNullOrEmpty(user.Email))
        {
             claims.Add(new Claim(ClaimTypes.Email, user.Email));
        }
       
        claims.Add(new Claim(ClaimTypes.Role, user.Role.ToString())); 
        
        var identity = new ClaimsIdentity(claims, "CustomAuthScheme"); 

        _currentUserPrincipal = new ClaimsPrincipal(identity);
    }

    public void ClearCurrentUser()
    {
        _currentUserPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
    }
}