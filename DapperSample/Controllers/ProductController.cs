using DapperSample.Repositroy;
using DapperSample.ResultDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DapperSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository= productRepository;
        }
        [HttpGet]
        public ActionResult<ApiResult<IEnumerable<ProductOutPutDto>>> GetProducts()
        {
            var result= _productRepository.GetProducts();
            Response.StatusCode = result.StatusCode;
            return result;
        }
    }
}
