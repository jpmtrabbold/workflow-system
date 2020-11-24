using System.Collections.Generic;
using Company.WorkflowSystem.Application.Models.Dtos.Products;
using Company.WorkflowSystem.Application.Models.ViewModels.Shared;

namespace Company.WorkflowSystem.Application.Models.ViewModels.Nodes
{
    public class ProductsListResponse : ListResponse
    {
        public List<ProductListDto> Products { get; set; }
    }
}
