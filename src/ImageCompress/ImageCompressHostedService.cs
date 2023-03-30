using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Volo.Abp;

namespace ImageCompress;

public class ImageCompressHostedService : IHostedService
{
    private readonly IAbpApplicationWithExternalServiceProvider _abpApplication;
    private readonly IImageCompressService _imageCompressService;

    public ImageCompressHostedService(IEnumerable<IImageCompressService> imageCompressServices,
        IConfiguration configuration,
        IAbpApplicationWithExternalServiceProvider abpApplication)
    {
        _imageCompressService = imageCompressServices.FirstOrDefault(x=>x.Name == configuration["ImageCompressService"]);
        _abpApplication = abpApplication;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _imageCompressService.CompressAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _abpApplication.ShutdownAsync();
    }
}
