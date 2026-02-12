namespace SimpleWPFWork.ApplicationContracts.Categories
{

    public interface ICategoryAppService
    {
        Task<CategoryDto> CreateAsync(CategoryCommand input);
        Task<CategoryDto> UpdateAsync(CategoryCommand input);
        Task DeleteAsync(Guid Id);
        Task<List<CategoryDto>> GetPaginationList(CategoryQuery input);
        Task<CategoryDto> GetAsync(Guid Id);
    }
}
