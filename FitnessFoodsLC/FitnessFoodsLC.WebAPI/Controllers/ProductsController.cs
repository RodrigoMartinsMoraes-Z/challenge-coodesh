using FitnessFoodsLC.Interface;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessFoodsLC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IOpenFoods _openFoods;
        private readonly IProductRepository _productRepository;

        public ProductsController(IOpenFoods openFoods, IProductRepository productRepository)
        {
            _openFoods = openFoods;
            _productRepository = productRepository;
        }

        [HttpGet]
        [Route("teste")]
        public async Task<ActionResult> Teste()
        {
            await _openFoods.GetFoods();

            return Ok();
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Challenge()
        {
            return Ok("Fullstack Challenge 20201026");
        }

        [HttpGet]
        [Route("{code}")]
        public async Task<ActionResult> FindByCode(long code)
        {
            return Ok(await _productRepository.FindByCode(code));
        }

        [HttpGet]
        [Route("{page}/{maxItens}")]
        public async Task<ActionResult> GetList(int page, int maxItens)
        {
            if (page <= 0)
                page = 1;

            if (maxItens < 0)
                return BadRequest("max itens must be positive greater than zero.");

            return Ok(await _productRepository.GetList(page, maxItens));

        }

        [HttpGet]
        [Route("{page}")]
        public async Task<ActionResult> GetList(int page)
        {
            if (page <= 0)
                page = 1;


            return Ok(await _productRepository.GetList(page));

        }
        [HttpGet]
        [Route("")]
        public async Task<ActionResult> GetList()
        {
            if (page <= 0)
                page = 1;


            return Ok(await _productRepository.GetList());

        }
    }
}
