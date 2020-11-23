using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Company.DealSystem.Application.Models.Dtos.DealCategories;
using Company.DealSystem.Application.Models.ViewModels.DealCategories;
using Company.DealSystem.Application.Services;

namespace Company.DealSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DealCategoryController : Controller
    {
        readonly DealCategoryService _service;
        public DealCategoryController(DealCategoryService service)
        {
            _service = service;
        }

        [HttpPost("List")]
        async public Task<DealCategoriesListResponse> List([FromBody] DealCategoriesListRequest listRequest) => await _service.List(listRequest);

        [HttpGet("{id}")]
        async public Task<DealCategoryDto> Get(int id) => await _service.Get(id);

        [HttpPost]
        async public Task<DealCategoryPostResponse> Post([FromBody] DealCategoryDto dealCategory)
        {
            return await _service.Save(dealCategory);
        }
    }
}