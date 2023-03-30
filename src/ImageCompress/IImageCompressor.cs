using System;
using System.IO;

namespace ImageCompress;

public interface IImageCompressor
{
    void Compress(Stream stream);
    void Compress(string path);
    bool IsSupported(Stream stream);
    bool IsSupported(string path);
}