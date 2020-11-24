using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Company.WorkflowSystem.Application.Models.Dtos.Products;
using Company.WorkflowSystem.Application.Models.ViewModels.Nodes;
using Company.WorkflowSystem.Application.Models.ViewModels.Shared;
using Company.WorkflowSystem.Application.Services;

namespace Company.WorkflowSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductController : Controller
    {
        readonly ProductService _service;
        public ProductController(ProductService service)
        {
            _service = service;
        }

        [HttpPost("List")]
        async public Task<ProductsListResponse> List([FromBody] ProductsListRequest listRequest) => await _service.List(listRequest);

        [HttpGet("{id}")]
        async public Task<ProductDto> Get(int id) => await _service.Get(id);

        [HttpPost]
        async public Task<ProductPostResponse> Post([FromBody] ProductDto node)
        {
            return await _service.Save(node);
        }

        [HttpPost("GetProducts")]
        async public Task<List<LookupRequest>> GetProducts(ProductDto node, int dealCategoryId, string query) =>
            await _service.GetProducts(node, dealCategoryId, query);
    }
}