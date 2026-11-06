using Microsoft.AspNetCore.Identity;
using NetCoreAdmin.Models;
using SqlSugar;

namespace NetCoreAdmin.Data;

public static class DbInitializer
{
    public static Task InitializeAsync(IServiceProvider services, IConfiguration configuration)
    {
        var db = services.GetRequiredService<ISqlSugarClient>();
        db.CodeFirst.InitTables<SysUser, SysRole, SysUserRole, SysMenu, SysRoleMenu,
            SysDictionaryType, SysDictionaryItem, SysConfig, SysAuditLog>();

        if (!db.Queryable<SysRole>().Any(x => x.Code == "SUPERADMIN"))
        {
            db.Insertable(new SysRole { Name = "Super administrator", Code = "SUPERADMIN", IsEnabled = true }).ExecuteCommand();
        }

        var userName = configuration["AdminSeed:UserName"] ?? "admin";
        if (!db.Queryable<SysUser>().Any(x => x.UserName == userName))
        {
            var user = new SysUser
            {
                UserName = userName,
                DisplayName = "Administrator",
                IsEnabled = true,
                CreatedAt = DateTime.UtcNow
            };
            var hasher = services.GetRequiredService<IPasswordHasher<SysUser>>();
            user.PasswordHash = hasher.HashPassword(user, configuration["AdminSeed:Password"] ?? "ChangeMe123!");
            db.Insertable(user).ExecuteCommand();
        }

        return Task.CompletedTask;
    }
}
