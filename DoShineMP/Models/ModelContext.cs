using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Models
{
    class ModelContext : DbContext
    {
        public ModelContext() : base("ModelContextConnString") { }

        public DbSet<Administrator> AdministratorSet { get; set; }

        public DbSet<File> FileSet { get; set; }

        public DbSet<Message> MessageSet { get; set; }

        public DbSet<Partner> PartnerSet { get; set; }

        public DbSet<Repair> RepairSet { get; set; }

        public DbSet<Token> TokenSet { get; set; }

        public DbSet<UserInfo> UserInfo { get; set; }

        public DbSet<WechatUser> WechatUserSet { get; set; }

        public DbSet<Terminal> TerminalSet { get; set; }

        public DbSet<ActiveLog> ActiveLogSet { get; set; }
    }
}
