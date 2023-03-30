using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Database;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace ImageCompress;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpBlobStoringModule),
    typeof(AbpEntityFrameworkCorePostgreSqlModule),
    typeof(BlobStoringDatabaseEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class ImageCompressModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);

        context.Services.AddAbpDbContext<ImageDbContext>();
        
        Configure<AbpDbContextOptions>(options =>
        {
            options.UseNpgsql<ImageDbContext>();
        });
        
        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureDefault(container =>
            {
                container.UseDatabase();
            });
        });
    }
}
