using SqlSugar;

namespace NetCoreAdmin.Models;

[SugarTable("sys_user")]
public class SysUser
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)] public long Id { get; set; }
    [SugarColumn(Length = 64, IsNullable = false)] public string UserName { get; set; } = "";
    [SugarColumn(Length = 255, IsNullable = false)] public string PasswordHash { get; set; } = "";
    [SugarColumn(Length = 100, IsNullable = false)] public string DisplayName { get; set; } = "";
    [SugarColumn(Length = 128, IsNullable = true)] public string? Email { get; set; }
    public bool IsEnabled { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

[SugarTable("sys_role")]
public class SysRole
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)] public long Id { get; set; }
    [SugarColumn(Length = 100, IsNullable = false)] public string Name { get; set; } = "";
    [SugarColumn(Length = 64, IsNullable = false)] public string Code { get; set; } = "";
    [SugarColumn(Length = 500, IsNullable = true)] public string? Description { get; set; }
    public bool IsEnabled { get; set; } = true;
}

[SugarTable("sys_user_role")]
public class SysUserRole
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)] public long Id { get; set; }
    public long UserId { get; set; }
    public long RoleId { get; set; }
}

[SugarTable("sys_menu")]
public class SysMenu
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)] public long Id { get; set; }
    public long? ParentId { get; set; }
    [SugarColumn(Length = 100, IsNullable = false)] public string Name { get; set; } = "";
    [SugarColumn(Length = 256, IsNullable = true)] public string? PagePath { get; set; }
    [SugarColumn(Length = 128, IsNullable = true)] public string? PermissionCode { get; set; }
    public int Sort { get; set; }
    public bool IsEnabled { get; set; } = true;
}

[SugarTable("sys_role_menu")]
public class SysRoleMenu
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)] public long Id { get; set; }
    public long RoleId { get; set; }
    public long MenuId { get; set; }
}

[SugarTable("sys_dictionary_type")]
public class SysDictionaryType
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)] public long Id { get; set; }
    [SugarColumn(Length = 64, IsNullable = false)] public string Code { get; set; } = "";
    [SugarColumn(Length = 100, IsNullable = false)] public string Name { get; set; } = "";
    public bool IsEnabled { get; set; } = true;
}

[SugarTable("sys_dictionary_item")]
public class SysDictionaryItem
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)] public long Id { get; set; }
    public long DictionaryTypeId { get; set; }
    [SugarColumn(Length = 64, IsNullable = false)] public string Code { get; set; } = "";
    [SugarColumn(Length = 100, IsNullable = false)] public string Label { get; set; } = "";
    [SugarColumn(Length = 256, IsNullable = true)] public string? Value { get; set; }
    public int Sort { get; set; }
    public bool IsEnabled { get; set; } = true;
}

[SugarTable("sys_config")]
public class SysConfig
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)] public long Id { get; set; }
    [SugarColumn(Length = 128, IsNullable = false)] public string Key { get; set; } = "";
    [SugarColumn(Length = 1000, IsNullable = true)] public string? Value { get; set; }
    [SugarColumn(Length = 500, IsNullable = true)] public string? Description { get; set; }
    public bool IsEnabled { get; set; } = true;
}

[SugarTable("sys_audit_log")]
public class SysAuditLog
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)] public long Id { get; set; }
    public long? UserId { get; set; }
    [SugarColumn(Length = 64, IsNullable = true)] public string? UserName { get; set; }
    [SugarColumn(Length = 128, IsNullable = false)] public string Action { get; set; } = "";
    [SugarColumn(Length = 1000, IsNullable = true)] public string? Detail { get; set; }
    [SugarColumn(Length = 64, IsNullable = true)] public string? IpAddress { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
