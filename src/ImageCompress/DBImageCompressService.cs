using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.BlobStoring.Database;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace ImageCompress;

public class DBImageCompressService : IImageCompressService, ITransientDependency
{
    public ILogger<DBImageCompressService> Logger { get; set; }
    private readonly IImageCompressor _magickImageCompressor;
    private readonly IRepository<DatabaseBlob, Guid> _blobRepository;
    private readonly IRepository<DatabaseBlobContainer> _blobContainerRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly string _blobContainerName;

    public DBImageCompressService(IImageCompressor magickImageCompressor, 
        IRepository<DatabaseBlob, Guid> blobRepository, 
        IRepository<DatabaseBlobContainer> blobContainerRepository,
        IUnitOfWorkManager unitOfWorkManager,
        IConfiguration configuration)
    {
        _magickImageCompressor = magickImageCompressor;
        _blobRepository = blobRepository;
        _blobContainerRepository = blobContainerRepository;
        _unitOfWorkManager = unitOfWorkManager;
        Logger = NullLogger<DBImageCompressService>.Instance;
        _blobContainerName = configuration["BlobContainerName"];
    }

    public string Name => "DB";

    public async Task CompressAsync()
    {
        using var uow = _unitOfWorkManager.Begin();
        var container = await _blobContainerRepository.GetAsync(x => x.Name == _blobContainerName);
        var blobs = await _blobRepository.GetListAsync(x => x.ContainerId == container.Id);
        
        foreach (var blob in blobs)
        {
            using var stream = new MemoryStream(blob.Content);
            _magickImageCompressor.Compress(stream);
            blob.SetContent(stream.ToArray());
        }
        await uow.SaveChangesAsync();
    }
}