using StopWatch.Repositories.Contract;
using Tesseract;

namespace StopWatch.Repositories
{
    public class ImageAnalysisRepository : IImageAnalysisRepository
    {
        public Task<string> AnalyseImage(string image)
        {
            var engine = new TesseractEngine(@"tessdata", "eng", EngineMode.Default);
            var img = Pix.LoadFromFile(@"Images/testocr.png");
            var page = engine.Process(img);

            var text = page.GetText();

            Console.WriteLine(text);

            return Task.FromResult(text);
        }
    }
}
