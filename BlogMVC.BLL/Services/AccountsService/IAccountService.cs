using BlogMVC.BLL.Models;
using BlogMVC.DAL.Models;
using BlogMVC.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BlogMVC.BLL.Services.AccountsService
{
    public interface IAccountService
    {
        Task<IdentityResult> Register(UserDTO user, string password);

        Task<SignInResult> Login(LoginDTO model);

        Task Logout();

        Task<User> GetUserAsync(ClaimsPrincipal user);
    }
}
