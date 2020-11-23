using System;
using System.Collections.Generic;
using System.Text;
using Company.DealSystem.Application.Models.Dtos.DealCategories;
using Company.DealSystem.Application.Models.ViewModels.Shared;

namespace Company.DealSystem.Application.Models.ViewModels.DealCategories
{
    public class DealCategoriesListResponse : ListResponse
    {
        public List<DealCategoryListDto> DealCategories { get; set; }
    }
}
