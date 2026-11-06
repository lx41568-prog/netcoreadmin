using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetCoreAdmin.Models;
using SqlSugar;

namespace NetCoreAdmin.Pages.Account;

public class LoginModel(ISqlSugarClient db, IPasswordHasher<SysUser> passwordHasher) : PageModel
{
    [BindProperty] public LoginInput Input { get; set; } = new();
    [TempData] public string? ErrorMessage { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        if (!ModelState.IsValid) return Page();
        var user = db.Queryable<SysUser>().First(x => x.UserName == Input.UserName && x.IsEnabled);
        if (user is null || passwordHasher.VerifyHashedPassword(user, user.PasswordHash, Input.Password) == PasswordVerificationResult.Failed)
        {
            ErrorMessage = "用户名或密码不正确，或账户已停用。";
            return Page();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName),
            new("display_name", user.DisplayName)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        return LocalRedirect(Url.IsLocalUrl(returnUrl) ? returnUrl! : Url.Page("/Index")!);
    }

    public sealed class LoginInput
    {
        [Required(ErrorMessage = "请输入用户名")]
        [StringLength(64)] public string UserName { get; set; } = "";
        [Required(ErrorMessage = "请输入密码")]
        [StringLength(128, MinimumLength = 8)] public string Password { get; set; } = "";
    }
}
