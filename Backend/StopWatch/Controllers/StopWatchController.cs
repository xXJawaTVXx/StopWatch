using Microsoft.AspNetCore.Mvc;
using StopWatch.DTOs;
using StopWatch.Repositories.Contract;

namespace StopWatch.Controllers
{
    [ApiController]
    [Route("stopwatch")]
    public class StopWatchController : ControllerBase
    {
        private readonly IImageAnalysisRepository _imageAnalysisRepository;

        public StopWatchController(IImageAnalysisRepository imageAnalysisRepository)
        {
            this._imageAnalysisRepository = imageAnalysisRepository;
        }

        [HttpPost("analyse")]
        public async Task<ActionResult<string>> UploadedImage([FromBody] string image)
        {
            try
            {
                var result = await this._imageAnalysisRepository.AnalyseImage(image);
                return this.Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}
