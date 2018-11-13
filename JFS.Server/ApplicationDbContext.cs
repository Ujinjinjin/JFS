using JFS.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace JFS
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<CommonField> CommonField { get; set; }
        public DbSet<Config> Config { get; set; }
        public DbSet<TfsConfig> TfsConfig { get; set; }
        public DbSet<JiraConfig> JiraConfig { get; set; }
        public DbSet<JiraField> JiraField { get; set; }
        public DbSet<Mapping> Mapping { get; set; }
        public DbSet<Profile> Profile { get; set; }
        public DbSet<Sync> Sync { get; set; }
        public DbSet<TfsField> TfsField { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=../Database/db.sqlite3");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommonField>(e =>
            {
                e.Property(cf => cf.Id).HasColumnName("id");
                e.Property(cf => cf.Name).HasColumnName("name");
                e.Property(cf => cf.Verbose).HasColumnName("verbose");
            });

            modelBuilder.Entity<Config>(e =>
            {
                e.Property(conf => conf.Id).HasColumnName("id");
                e.Property(conf => conf.ProfileId).HasColumnName("profile_id");
                e.Property(conf => conf.TfsConfigId).HasColumnName("tfs_config_id");
                e.Property(conf => conf.JiraConfigId).HasColumnName("jira_config_id");
            });

            modelBuilder.Entity<TfsConfig>(e =>
            {
                e.Property(conf => conf.Id).HasColumnName("id");
                e.Property(conf => conf.Priority).HasColumnName("priority");
                e.Property(conf => conf.ParentId).HasColumnName("parent_id");
                e.Property(conf => conf.Iteration).HasColumnName("sprint");
                e.Property(conf => conf.Area).HasColumnName("area");
                e.Property(conf => conf.TeamProject).HasColumnName("team_project");
            });

            modelBuilder.Entity<JiraConfig>(e =>
            {
                e.Property(conf => conf.Id).HasColumnName("id");
                e.Property(conf => conf.Priority).HasColumnName("priority");
                e.Property(conf => conf.Sprint).HasColumnName("sprint");
            });

            modelBuilder.Entity<JiraField>(e =>
            {
                e.Property(jf => jf.Id).HasColumnName("id");
                e.Property(jf => jf.Name).HasColumnName("name");
                e.Property(jf => jf.Verbose).HasColumnName("verbose");
            });

            modelBuilder.Entity<Mapping>(e =>
            {
                e.Property(m => m.Id).HasColumnName("id");
                e.Property(m => m.ProfileId).HasColumnName("profile_id");
                e.Property(m => m.CommonFieldId).HasColumnName("common_field_id");
                e.Property(m => m.TfsFieldId).HasColumnName("tfs_field_id");
                e.Property(m => m.JiraFieldId).HasColumnName("jira_field_id");
            });

            modelBuilder.Entity<Profile>(e =>
            {
                e.Property(p => p.Id).HasColumnName("id");
                e.Property(p => p.Name).HasColumnName("name");
                e.Property(p => p.Active).HasColumnName("active");
            });

            modelBuilder.Entity<Sync>(e =>
            {
                e.Property(s => s.Id).HasColumnName("id");
                e.Property(s => s.JiraId).HasColumnName("jira_id");
                e.Property(s => s.TfsId).HasColumnName("tfs_id");
                e.Property(s => s.Rev).HasColumnName("rev");
                e.Property(s => s.Deleted).HasColumnName("deleted");
            });

            modelBuilder.Entity<TfsField>(e =>
            {
                e.Property(tf => tf.Id).HasColumnName("id");
                e.Property(tf => tf.Name).HasColumnName("name");
                e.Property(tf => tf.Verbose).HasColumnName("verbose");
            });
        }
    }
}
