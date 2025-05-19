using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

namespace El_Harrifa.Helpers
{
    public static class ImageOptimizer
    {
        public static async Task<string> OptimizeImageAsync(IFormFile file, string outputPath, int maxWidth = 800, int quality = 80)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!IsValidImageExtension(extension))
                throw new ArgumentException("Invalid file type");

            var fileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(outputPath, fileName);

            using (var image = await Image.LoadAsync(file.OpenReadStream()))
            {
                // Resize if needed
                if (image.Width > maxWidth)
                {
                    var ratio = (float)maxWidth / image.Width;
                    var newHeight = (int)(image.Height * ratio);
                    image.Mutate(x => x.Resize(maxWidth, newHeight));
                }

                // Save optimized image
                using (var outputStream = new FileStream(fullPath, FileMode.Create))
                {
                    if (extension == ".jpg" || extension == ".jpeg")
                    {
                        await image.SaveAsJpegAsync(outputStream, new JpegEncoder { Quality = quality });
                    }
                    else if (extension == ".png")
                    {
                        await image.SaveAsPngAsync(outputStream, new PngEncoder { CompressionLevel = 6 });
                    }
                }
            }

            return fileName;
        }

        private static bool IsValidImageExtension(string extension)
        {
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif";
        }
    }
} 