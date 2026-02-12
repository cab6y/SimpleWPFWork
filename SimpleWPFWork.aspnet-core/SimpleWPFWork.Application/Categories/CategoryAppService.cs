using SimpleWPFWork.ApplicationContracts.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleWPFWork.Application.Categories
{
    public class CategoryAppService : ICategoryAppService
    {
        public CategoryAppService()
        {

        }

        public Task<CategoryDto> CreateAsync(CategoryCommand input)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDto> GetAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<List<CategoryDto>> GetPaginationList(CategoryQuery input)
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDto> UpdateAsync(CategoryCommand input)
        {
            throw new NotImplementedException();
        }
    }
}
