using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using Accord.Video.VFW;
using Accord.Imaging;


namespace YTStorage.Services
{
    class Program
    {
        public static string[] BinaryStringFromByteArray(byte[] array)
        {
            string binaryString = string.Join(" ", array.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));

            return binaryString.Split(" ");
        }

        public static uint[] BinaryToRGB(string[] binary)
        {
            uint c = 0;
            uint[] rgb = new uint[binary.Length];

            foreach (string i in binary)
            {
                rgb[c++] = (uint)Convert.ToInt16(i, 2);
            }

            return rgb;
        }

        public static Bitmap GenerateFrame(int height, int width)
        {
            return new Bitmap(width, height);
        }

        public static Bitmap ModifyFrame(int height, int width, Bitmap bitmap, uint[] RGB) 
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Color color = Color.FromArgb((int)RGB[0], (int)RGB[1], (int)RGB[2]);
                    bitmap.SetPixel(i, j, color);
                }
            }

            return bitmap;
        }

        static async Task Main() 
        {
            string sourceFilePath = "C:\\Users\\Kacper\\Downloads\\diskgeeker.dmg\\maxverstappen.txt";
            string destinationFilePath = "C:\\Users\\Kacper\\Downloads\\diskgeeker.dmg\\output.avi";

            await FromFileToVideo(sourceFilePath, destinationFilePath);

            Console.WriteLine("Video creation complete.");
        }

        static async Task FromFileToVideo(string sourceFilePath, string destinationFilePath)
        {
            // Set video parameters (you may need to adjust these based on your requirements)
            var videoWidth = 640;
            var videoHeight = 480;

            // Create AVI writer
            var aviWriter = new Accord.Video.VFW.AVIWriter("XVID");
            aviWriter.Open(destinationFilePath, videoWidth, videoHeight);
            Bitmap image_tmp  = null;
            uint counter = 0;
            bool checker = false;

            using (FileStream sourceStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
            {
                byte[] buffer = new byte[3];
                int bytesRead;

                while ((bytesRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    uint[] rgbRepresentation = BinaryToRGB(
                        BinaryStringFromByteArray(buffer)
                    );

                    if(counter == videoWidth * videoHeight)
                    {
                        checker = true;
                    }

                    if(image_tmp == null)
                    {
                        image_tmp = GenerateFrame(videoHeight, videoWidth);
                    }

                    ModifyFrame(videoHeight, videoWidth, image_tmp, rgbRepresentation);

                    if (checker)
                    {
                        checker = false;
                        aviWriter.AddFrame(new Accord.Imaging.RGB(rgbRepresentation[0], rgbRepresentation[1], rgbRepresentation[2]));
                        image_tmp = null;
                    }


                    
                }
            }

            aviWriter.Close();
        }
    }
}
