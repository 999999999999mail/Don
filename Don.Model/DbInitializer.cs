using Don.Infrastructure.Extensions;
using Don.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Don.Model
{
    public static class DbInitializer
    {
        public static void Initialize(DonContext context)
        {
            context.Database.EnsureCreated();
            // Look for any Admins.
            if (context.Admins.Any())
            {
                return; //Db has been seeded
            }

            #region 系统数据初始化
            // 初始化角色
            var role = new Role { Name = "系统管理员", CreateTime = DateTime.Now, Remark = "全权限管理员", Sys = true };
            context.Roles.Add(role);
            // 初始化管理员账号
            var admin = new Admin { LoginName = "admin", Password = "111111".ToMD5(), CreateTime = DateTime.Now };
            context.Admins.Add(admin);
            context.SaveChanges();
            // 初始化管理员角色
            context.AdminRoles.Add(new AdminRole { AdminId = admin.Id, RoleId = role.Id });
            // 初始化系统默认用户分组
            context.Groups.Add(new Group { Name = "未分组", CreateTime = DateTime.Now, Remark = "系统默认用户分组", Sys = true });
            context.SaveChanges();
            #endregion
        }
    }
}
