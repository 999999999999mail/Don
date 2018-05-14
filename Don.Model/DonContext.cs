using Don.Model.Entities;
using Don.Model.Mappings;
using Microsoft.EntityFrameworkCore;
using System;

namespace Don.Model
{
    public class DonContext : DbContext
    {
        public DonContext(DbContextOptions<DonContext> options)
            : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<AdminRole> AdminRoles { get; set; }

        public DbSet<AdminLog> AdminLogs { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Menu> Menus { get; set; }

        public DbSet<RoleMenu> RoleMenus { get; set; }

        public DbSet<UserLog> UserLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AdminMap());
            modelBuilder.ApplyConfiguration(new RoleMap());
            modelBuilder.ApplyConfiguration(new AdminRoleMap());
            modelBuilder.ApplyConfiguration(new AdminLogMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new GroupMap());
            modelBuilder.ApplyConfiguration(new MenuMap());
            modelBuilder.ApplyConfiguration(new RoleMenuMap());
            modelBuilder.ApplyConfiguration(new UserLogMap());
        }
    }
}
