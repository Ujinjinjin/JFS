using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JFS.Models.Db
{
    public class Config
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public Profile Profile { get; set; }
        public int TfsConfigId { get; set; }
        public TfsConfig TfsConfig { get; set; }
        public int JiraConfigId { get; set; }
        public JiraConfig JiraConfig { get; set; }

        public static async Task<Config> GetConfig(ApplicationDbContext context, int tfsPriority)
        {
            var config = context.Config
                .Include(c => c.TfsConfig)
                .Include(c => c.JiraConfig)
                .FirstOrDefault(c => c.Profile.Active && c.TfsConfig.Priority == tfsPriority);
            Thread.Sleep(5000);
            if (config != null)
                await context.Entry(config).ReloadAsync();
            return config;
        }

        public static async Task<Config> GetConfig(ApplicationDbContext context, string jiraPriority)
        {
            var config = context.Config
                .Include(c => c.TfsConfig)
                .Include(c => c.JiraConfig)
                .FirstOrDefault(c => c.Profile.Active && c.JiraConfig.Priority == jiraPriority);
            Thread.Sleep(5000);
            if (config != null)
                await context.Entry(config).ReloadAsync();
            return config;
        }
    }
}
