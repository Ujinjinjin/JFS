﻿using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public static Config GetConfig(ApplicationDbContext context)
        {
            return context.Config
                .Include(c => c.TfsConfig)
                .Include(c => c.JiraConfig)
                .First(c => c.Profile.Active);
        }
    }
}
