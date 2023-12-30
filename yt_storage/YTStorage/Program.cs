using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
namespace YTStorage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int c = 0;
            using (Image<Rgba32> image = Image.Load<Rgba32>("C:\\Users\\Kacper\\Downloads\\791bfc21-d6b9-4076-b76e-95fbf23effc5.png"))
            {
                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        c++;
                        Rgba32 pixelColor = image[x, y];

                        byte red = pixelColor.R;
                        byte green = pixelColor.G;
                        byte blue = pixelColor.B;

                        Console.WriteLine(c / (image.Height * image.Width) * 100);
                    }
                }
            }
        }
    }
}
