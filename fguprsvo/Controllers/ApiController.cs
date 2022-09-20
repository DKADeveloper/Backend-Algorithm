using fguprsvo.Alogs;
using Microsoft.AspNetCore.Mvc;

namespace fguprsvo.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        //Контроллер contrast
        [Route("contrast")]
        [HttpPost]
        public async Task<ActionResult> ApplyContrast(IFormFile file, string strContrast, bool isGrayscale)
        {
            //Инициализация класса ContrastAlgorithm
            ContrastAlgorithm algo = new()
            {
                File = file,
                IsGrayscale = isGrayscale,
                StrContrast = strContrast
            };

            var imgBase64 = await algo.RunAsync();

            return Ok(new { Status = "ok", img = imgBase64 });
        }

        //Контроллер gradient
        [Route("gradient")]
        [HttpPost]
        public async Task<ActionResult> ApplyGradient(IFormFile file, string algoType)
        {
            //Инициализация класса GradientAlgorithm
            GradientAlgorithm algo = new()
            {
                File = file,
                AlgoType = algoType
            };

            var imgBase64 = await algo.RunAsync();

            return Ok(new { Status = "ok", img = imgBase64 });
        }
    }
}
