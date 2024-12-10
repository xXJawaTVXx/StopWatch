using StopWatch.Repositories.Contract;
using Tesseract;

namespace StopWatch.Repositories
{
    public class ImageAnalysisRepository : IImageAnalysisRepository
    {
        public Task<string> AnalyseImage(string image)
        {
            var engine = new TesseractEngine(@"tessdata", "eng", EngineMode.Default);
            
            byte[] imageBytes = Convert.FromBase64String(image);
            MemoryStream memoryStream = new MemoryStream(imageBytes);
            Pix pixImage = Pix.LoadFromMemory(memoryStream.ToArray());
            var page = engine.Process(pixImage);

            var text = page.GetText();

            Console.WriteLine(text);

            return Task.FromResult(text);
        }
    }
}
