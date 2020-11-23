using System.Collections.Generic;
using Company.DealSystem.Application.Models.Dtos.Products;
using Company.DealSystem.Application.Models.ViewModels.Shared;

namespace Company.DealSystem.Application.Models.ViewModels.Nodes
{
    public class ProductsListResponse : ListResponse
    {
        public List<ProductListDto> Products { get; set; }
    }
}
