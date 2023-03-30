using Microsoft.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace ImageCompress;

[ReplaceDbContext(typeof(IBlobStoringDbContext))]
public class ImageDbContext : AbpDbContext<ImageDbContext> , IBlobStoringDbContext
{
    public ImageDbContext(DbContextOptions<ImageDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureBlobStoring();
    }

    public DbSet<DatabaseBlobContainer> BlobContainers { get; }
    public DbSet<DatabaseBlob> Blobs { get; }
}