using System.IO;
using ImageMagick;
using Volo.Abp.DependencyInjection;

namespace ImageCompress;

public class MagickImageCompressor : IImageCompressor, ITransientDependency
{
    private readonly ImageOptimizer _imageOptimizer;

    public MagickImageCompressor()
    {
        _imageOptimizer = new ImageOptimizer
        {
            OptimalCompression = true,
            IgnoreUnsupportedFormats = true,
        };
    }

    public void Compress(string path)
    {
        try
        {
            _imageOptimizer.Compress(path);
            _imageOptimizer.LosslessCompress(path);
        }
        catch
        {
            // ignored
        }
    }

    public bool IsSupported(Stream stream)
    {
        try
        {
            return _imageOptimizer.IsSupported(stream);
        }
        catch
        {
            return false;
        }
    }

    public bool IsSupported(string path)
    {
        try
        {
            return _imageOptimizer.IsSupported(path);
        }
        catch
        {
            return false;
        }
    }

    public void Compress(Stream stream)
    {
        try
        {
            _imageOptimizer.Compress(stream);
            _imageOptimizer.LosslessCompress(stream);
        }
        catch
        {
            // ignored
        }
    }
}