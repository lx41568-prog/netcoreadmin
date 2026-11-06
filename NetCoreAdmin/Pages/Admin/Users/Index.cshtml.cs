using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetCoreAdmin.Models;
using SqlSugar;

namespace NetCoreAdmin.Pages.Admin.Users;

public class IndexModel(ISqlSugarClient db, IPasswordHasher<SysUser> passwordHasher) : PageModel
{
    public List<SysUser> Users { get; private set; } = [];
    [TempData] public string? Message { get; set; }
    [BindProperty] public NewUserInput NewUser { get; set; } = new();

    public void OnGet() => LoadUsers();

    public IActionResult OnPostCreate()
    {
        if (!ModelState.IsValid) { LoadUsers(); return Page(); }
        if (db.Queryable<SysUser>().Any(x => x.UserName == NewUser.UserName.Trim()))
        {
            ModelState.AddModelError("NewUser.UserName", "用户名已存在。");
            LoadUsers();
            return Page();
        }

        var user = new SysUser
        {
            UserName = NewUser.UserName.Trim(),
            DisplayName = NewUser.DisplayName.Trim(),
            Email = string.IsNullOrWhiteSpace(NewUser.Email) ? null : NewUser.Email.Trim(),
            IsEnabled = true,
            CreatedAt = DateTime.UtcNow
        };
        user.PasswordHash = passwordHasher.HashPassword(user, NewUser.Password);
        db.Insertable(user).ExecuteCommand();
        Message = "用户已创建。";
        return RedirectToPage();
    }

    private void LoadUsers() => Users = db.Queryable<SysUser>().OrderBy(x => x.Id, OrderByType.Desc).ToList();

    public sealed class NewUserInput
    {
        [Required, StringLength(64, MinimumLength = 3)] public string UserName { get; set; } = "";
        [Required, StringLength(100)] public string DisplayName { get; set; } = "";
        [Required, StringLength(128, MinimumLength = 8)] public string Password { get; set; } = "";
        [EmailAddress, StringLength(128)] public string? Email { get; set; }
    }
}
