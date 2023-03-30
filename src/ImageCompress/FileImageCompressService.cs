using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace ImageCompress;

public class FileImageCompressService : IImageCompressService, ITransientDependency
{
    public ILogger<FileImageCompressService> Logger { get; set; }
    private readonly MagickImageCompressor _magickImageCompressor;

    public FileImageCompressService(MagickImageCompressor magickImageCompressor)
    {
        _magickImageCompressor = magickImageCompressor;
        Logger = NullLogger<FileImageCompressService>.Instance;
    }

    public string Name => "File";

    public Task CompressAsync()
    {
        Console.WriteLine("Enter the path of the folder of the image or images to be compressed:");
        var path = Console.ReadLine();
        if (string.IsNullOrEmpty(path))
        {
            Console.WriteLine("The path cannot be empty.");
            return Task.CompletedTask;
        }

        var filePaths = new List<string>();
        if (File.Exists(path))
        {
            filePaths.Add(path);
        }
        else if (Directory.Exists(path))
        {
            filePaths.AddRange(Directory.GetFiles(path));
        }
        else
        {
            Console.WriteLine("The path is not valid.");
            return Task.CompletedTask;
        }

        int i = 0;
        foreach (var filePath in filePaths)
        {
            if (_magickImageCompressor.IsSupported(filePath))
            {
                
                _magickImageCompressor.Compress(filePath);
                
                var fileName = Path.GetFileName(filePath);
                
                Console.WriteLine($"The image {fileName} has been compressed.");

                if (filePaths.Count > 1)
                {
                    Console.WriteLine($"Compressed image {++i} of {filePaths.Count}.");
                }
            }
        }
        
        return Task.CompletedTask;
    }
}