using System.Threading.Tasks;

namespace ImageCompress;

public interface IImageCompressService
{
    string Name { get; }
    Task CompressAsync();
}