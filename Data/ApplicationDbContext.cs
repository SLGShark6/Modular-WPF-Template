using Microsoft.EntityFrameworkCore;

namespace ModularWPFTemplate.Data
{
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Example data set
        /// </summary>
        // public DbSet<Example> Settings { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        /// <summary>
        /// Entity Configurations
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
