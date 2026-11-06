using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetCoreAdmin.Models;
using SqlSugar;

namespace NetCoreAdmin.Pages.Admin.Configs;

public class IndexModel(ISqlSugarClient db) : PageModel
{
    public List<SysConfig> Configs { get; private set; } = [];
    [BindProperty] public ConfigInput Input { get; set; } = new();
    [TempData] public string? Message { get; set; }
    public void OnGet() => Load();
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) { Load(); return Page(); }
        var key = Input.Key.Trim();
        var existing = db.Queryable<SysConfig>().First(x => x.Key == key);
        if (existing is null) db.Insertable(new SysConfig { Key = key, Value = Input.Value, Description = Input.Description, IsEnabled = true }).ExecuteCommand();
        else { existing.Value = Input.Value; existing.Description = Input.Description; db.Updateable(existing).ExecuteCommand(); }
        Message = "系统参数已保存。"; return RedirectToPage();
    }
    private void Load() => Configs = db.Queryable<SysConfig>().OrderBy(x => x.Key).ToList();
    public sealed class ConfigInput { [Required, StringLength(128)] public string Key { get; set; } = ""; [StringLength(1000)] public string? Value { get; set; } [StringLength(500)] public string? Description { get; set; } }
}
