using JFS.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace JFS
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<CommonField> CommonField { get; set; }
        public DbSet<Config> Config { get; set; }
        public DbSet<JiraField> JiraField { get; set; }
        public DbSet<Mapping> Mapping { get; set; }
        public DbSet<Profile> Profile { get; set; }
        public DbSet<Sync> Sync { get; set; }
        public DbSet<TfsField> TfsField { get; set; }
        public DbSet<WorkItemType> WorkItemType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=../Database/db.sqlite3");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommonField>(e =>
            {
                e.Property(cf => cf.Id).HasField("id");
                e.Property(cf => cf.Name).HasField("name");
                e.Property(cf => cf.Verbose).HasField("verbose");
            });

            modelBuilder.Entity<Config>(e =>
            {
                e.Property(conf => conf.Id).HasField("id");
                e.Property(conf => conf.ProfileId).HasField("profile_id");
                e.Property(conf => conf.Priority).HasField("priority");
                e.Property(conf => conf.Sprint).HasField("sprint");
                e.Property(conf => conf.WorkItemTypeId).HasField("work_item_type_id");
            });

            modelBuilder.Entity<JiraField>(e =>
            {
                e.Property(jf => jf.Id).HasField("id");
                e.Property(jf => jf.Name).HasField("name");
                e.Property(jf => jf.Verbose).HasField("verbose");
            });

            modelBuilder.Entity<Mapping>(e =>
            {
                e.Property(m => m.Id).HasField("id");
                e.Property(m => m.ProfileId).HasField("profile_id");
                e.Property(m => m.CommonFieldId).HasField("common_field_id");
                e.Property(m => m.TfsFieldId).HasField("tfs_field_id");
                e.Property(m => m.JiraFieldId).HasField("jira_field_id");
            });

            modelBuilder.Entity<Profile>(e =>
            {
                e.Property(p => p.Id).HasField("id");
                e.Property(p => p.Name).HasField("name");
                e.Property(p => p.Active).HasField("active");
            });

            modelBuilder.Entity<Sync>(e =>
            {
                e.Property(s => s.Id).HasField("id");
                e.Property(s => s.JiraId).HasField("jira_id");
                e.Property(s => s.TfsId).HasField("tfs_id");
            });

            modelBuilder.Entity<TfsField>(e =>
            {
                e.Property(tf => tf.Id).HasField("id");
                e.Property(tf => tf.Name).HasField("name");
                e.Property(tf => tf.Verbose).HasField("verbose");
            });

            modelBuilder.Entity<WorkItemType>(e =>
            {
                e.Property(wi => wi.Id).HasField("id");
                e.Property(wi => wi.Type).HasField("type");
            });
        }
    }
}
