namespace StopWatch.Repositories.Contract
{
    public interface IImageAnalysisRepository
    {
        Task<string> AnalyseImage(string image);
    }
}
