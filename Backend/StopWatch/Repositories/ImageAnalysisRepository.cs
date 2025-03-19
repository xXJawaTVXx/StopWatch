using System.Text.RegularExpressions;
using SkiaSharp;
using StopWatch.Repositories.Contract;
using Tesseract;

namespace StopWatch.Repositories
{
    public class ImageAnalysisRepository : IImageAnalysisRepository
    {
        private readonly string templatePath = @"Images/template.jpg";

        public Task<string> AnalyseImage(string image)
        {
            var engine = new TesseractEngine(@"tessdata", "eng", EngineMode.Default);
            engine.SetVariable("tessedit_char_whitelist", "0123456789:");
            engine.SetVariable("classify_bln_numeric_mode", "1");

            byte[] imageBytes = Convert.FromBase64String(image);
            MemoryStream memoryStream = new MemoryStream(imageBytes);

            SKBitmap bitmapImage = SKBitmap.Decode(memoryStream);

            float contrast = 50.0f;
            SKBitmap contrastAdjustedBitmap = AdjustContrast(bitmapImage, contrast);

            byte[] adjustedImageBytes = SKBitmapToByteArray(contrastAdjustedBitmap);

            Pix pixImage = Pix.LoadFromMemory(adjustedImageBytes);
            var page = engine.Process(pixImage);

            var text = page.GetText();

            // Aktuelles Format mm:ss:msms
            string pattern = @"(\d{1,2})\s*:\s*(\d{2})\s*:\s*(\d{2})";
            Match match = Regex.Match(text, pattern);
            
            Console.WriteLine("----------------------------------");
            Console.WriteLine("Minuten: " + match.Groups[1].Value);
            Console.WriteLine("Sekunden: " + match.Groups[2].Value);
            Console.WriteLine("Milisekunden: " + match.Groups[3].Value);
            Console.WriteLine("Gesamte Zeit: " + match.Value);
            Console.WriteLine("----------------------------------");

            return Task.FromResult(match.Value);
        }

        private static SKBitmap AdjustContrast(SKBitmap image, float contrast)
        {
            float adjustedContrast = (259 * (contrast + 255)) / (255 * (259 - contrast));

            SKBitmap contrastImage = new SKBitmap(image.Width, image.Height);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    SKColor pixel = image.GetPixel(x, y);

                    byte AdjustValue(byte color, float contrastFactor)
                    {
                        float adjusted = ((color / 255.0f - 0.5f) * contrastFactor + 0.5f) * 255;
                        return (byte)Math.Clamp(adjusted, 0, 255);
                    }

                    byte red = AdjustValue(pixel.Red, adjustedContrast);
                    byte green = AdjustValue(pixel.Green, adjustedContrast);
                    byte blue = AdjustValue(pixel.Blue, adjustedContrast);

                    contrastImage.SetPixel(x, y, new SKColor(red, green, blue, pixel.Alpha));
                }
            }

            return contrastImage;
        }

        private static byte[] SKBitmapToByteArray(SKBitmap bitmap)
        {
            using (var image = SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            {
                return data.ToArray();
            }
        }
    }
}

