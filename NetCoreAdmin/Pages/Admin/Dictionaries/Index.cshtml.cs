using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetCoreAdmin.Models;
using SqlSugar;

namespace NetCoreAdmin.Pages.Admin.Dictionaries;

public class IndexModel(ISqlSugarClient db) : PageModel
{
    public List<SysDictionaryType> Types { get; private set; } = [];
    public List<SysDictionaryItem> Items { get; private set; } = [];
    [BindProperty] public TypeForm TypeInput { get; set; } = new();
    [BindProperty] public ItemForm ItemInput { get; set; } = new();
    [TempData] public string? Message { get; set; }
    public void OnGet() => Load();
    public IActionResult OnPostCreateType()
    {
        if (!ModelState.IsValid) { Load(); return Page(); }
        var code = TypeInput.Code.Trim();
        if (db.Queryable<SysDictionaryType>().Any(x => x.Code == code)) { ModelState.AddModelError("TypeInput.Code", "类型代码已存在。"); Load(); return Page(); }
        db.Insertable(new SysDictionaryType { Name = TypeInput.Name.Trim(), Code = code, IsEnabled = true }).ExecuteCommand();
        Message = "字典类型已创建。"; return RedirectToPage();
    }
    public IActionResult OnPostCreateItem()
    {
        if (!ModelState.IsValid) { Load(); return Page(); }
        if (!db.Queryable<SysDictionaryType>().Any(x => x.Id == ItemInput.DictionaryTypeId)) { ModelState.AddModelError("ItemInput.DictionaryTypeId", "请选择有效字典类型。"); Load(); return Page(); }
        db.Insertable(new SysDictionaryItem { DictionaryTypeId = ItemInput.DictionaryTypeId, Label = ItemInput.Label.Trim(), Code = ItemInput.Code.Trim(), Value = ItemInput.Value?.Trim(), IsEnabled = true }).ExecuteCommand();
        Message = "字典项已添加。"; return RedirectToPage();
    }
    private void Load() { Types = db.Queryable<SysDictionaryType>().OrderBy(x => x.Id).ToList(); Items = db.Queryable<SysDictionaryItem>().OrderBy(x => x.DictionaryTypeId).OrderBy(x => x.Sort).ToList(); }
    public sealed class TypeForm { [Required, StringLength(100)] public string Name { get; set; } = ""; [Required, StringLength(64)] public string Code { get; set; } = ""; }
    public sealed class ItemForm { [Range(1, long.MaxValue)] public long DictionaryTypeId { get; set; } [Required, StringLength(100)] public string Label { get; set; } = ""; [Required, StringLength(64)] public string Code { get; set; } = ""; [StringLength(256)] public string? Value { get; set; } }
}
