using FtpPoller.Data.Entities;
using FtpPollerService.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace FtpPoller.Data
{
    public class FtpPollerContext: DbContext
    {
        public FtpPollerContext(DbContextOptions<FtpPollerContext> options)
           : base(options)
        {

        }
        public DbSet<Entities.File> CopiedFiles { get; set; }
        public DbSet<PayerServer> PayerServers { get; set; }
        public DbSet<RecipientServer> RecipientServers { get; set; }
        public DbSet<Subscriptions> Subscriptions { get; set; }
    }
}