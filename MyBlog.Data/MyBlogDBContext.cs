using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using MyBlog.Data.Models;

namespace MyBlog.Data
{
    public class MyBlogDbContext : ApiAuthorizationDbContext<AppUser>
    {
        public MyBlogDbContext(DbContextOptions options) : base(options, new OperationalStoreOptionsMigrations())
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
      
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }

    public class OperationalStoreOptionsMigrations : IOptions<OperationalStoreOptions>
    {
        public OperationalStoreOptions Value => new OperationalStoreOptions()
        {
            DeviceFlowCodes = new TableConfiguration("DeviceCodes"),
            EnableTokenCleanup = false,
            PersistedGrants = new TableConfiguration("PersistedGrants"),
            TokenCleanupBatchSize = 100,
            TokenCleanupInterval = 3600,
        };
    }

    public class MyBlogDbContextFactory : IDesignTimeDbContextFactory<MyBlogDbContext>
    {
        public MyBlogDbContext CreateDbContext
          (string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyBlogDbContext>();
            optionsBuilder.UseSqlite("Data Source = ../MyBlog.db");
            return new MyBlogDbContext
              (optionsBuilder.Options);
        }
    }
}
