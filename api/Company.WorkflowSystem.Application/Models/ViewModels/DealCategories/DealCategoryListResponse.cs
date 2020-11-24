using System;
using System.Collections.Generic;
using System.Text;
using Company.WorkflowSystem.Application.Models.Dtos.DealCategories;
using Company.WorkflowSystem.Application.Models.ViewModels.Shared;

namespace Company.WorkflowSystem.Application.Models.ViewModels.DealCategories
{
    public class DealCategoriesListResponse : ListResponse
    {
        public List<DealCategoryListDto> DealCategories { get; set; }
    }
}
