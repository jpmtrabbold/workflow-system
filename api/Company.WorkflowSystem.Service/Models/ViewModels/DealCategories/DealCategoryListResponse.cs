using System;
using System.Collections.Generic;
using System.Text;
using Company.WorkflowSystem.Service.Models.Dtos.DealCategories;
using Company.WorkflowSystem.Service.Models.ViewModels.Shared;

namespace Company.WorkflowSystem.Service.Models.ViewModels.DealCategories
{
    public class DealCategoriesListResponse : ListResponse
    {
        public List<DealCategoryListDto> DealCategories { get; set; }
    }
}
