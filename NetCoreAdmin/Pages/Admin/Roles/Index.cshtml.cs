using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetCoreAdmin.Models;
using SqlSugar;

namespace NetCoreAdmin.Pages.Admin.Roles;

public class IndexModel(ISqlSugarClient db) : PageModel
{
    public List<SysRole> Roles { get; private set; } = [];
    [BindProperty] public RoleInput Input { get; set; } = new();
    [TempData] public string? Message { get; set; }
    public void OnGet() => Load();
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) { Load(); return Page(); }
        var code = Input.Code.Trim().ToUpperInvariant();
        if (db.Queryable<SysRole>().Any(x => x.Code == code))
        {
            ModelState.AddModelError("Input.Code", "角色代码已存在。"); Load(); return Page();
        }
        db.Insertable(new SysRole { Name = Input.Name.Trim(), Code = code, Description = Input.Description?.Trim(), IsEnabled = true }).ExecuteCommand();
        Message = "角色已创建。"; return RedirectToPage();
    }
    private void Load() => Roles = db.Queryable<SysRole>().OrderBy(x => x.Id).ToList();
    public sealed class RoleInput
    {
        [Required, StringLength(100)] public string Name { get; set; } = "";
        [Required, RegularExpression("^[A-Za-z0-9_-]+$")] public string Code { get; set; } = "";
        [StringLength(500)] public string? Description { get; set; }
    }
}
