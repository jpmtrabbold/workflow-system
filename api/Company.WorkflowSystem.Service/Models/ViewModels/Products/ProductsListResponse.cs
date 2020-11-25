using System.Collections.Generic;
using Company.WorkflowSystem.Service.Models.Dtos.Products;
using Company.WorkflowSystem.Service.Models.ViewModels.Shared;

namespace Company.WorkflowSystem.Service.Models.ViewModels.Nodes
{
    public class ProductsListResponse : ListResponse
    {
        public List<ProductListDto> Products { get; set; }
    }
}
