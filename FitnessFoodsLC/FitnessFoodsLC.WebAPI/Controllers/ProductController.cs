using FitnessFoodsLC.Interface;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessFoodsLC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IOpenFoods _openFoods;

        public ProductController(IOpenFoods openFoods)
        {
            _openFoods = openFoods;
        }

        [HttpGet]
        public async Task<ActionResult> Teste()
        {
            await _openFoods.GetFoods();

            return Ok();
        }
    }
}
