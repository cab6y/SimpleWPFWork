using SimpleWPFWork.ApplicationContracts.Categories.Commands.CreateCategory;
using SimpleWPFWork.ApplicationContracts.Categories.Commands.UpdateCategory;
using SimpleWPFWork.ApplicationContracts.Categories.Commands.DeleteCategory;
using SimpleWPFWork.ApplicationContracts.Categories.Queries.GetCategory;
using SimpleWPFWork.ApplicationContracts.Categories.Queries.GetCategoryList;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleWPFWork.ApplicationContracts.Categories
{
    public interface ICategoryAppService
    {
        Task<CategoryDto> CreateAsync(CreateCategoryCommand input);
        Task<CategoryDto> UpdateAsync(UpdateCategoryCommand input);
        Task<CategoryDto> GetAsync(Guid id);
        Task<List<CategoryDto>> GetPaginationListAsync(GetCategoryListQuery input);
        Task DeleteAsync(Guid id);
    }
}