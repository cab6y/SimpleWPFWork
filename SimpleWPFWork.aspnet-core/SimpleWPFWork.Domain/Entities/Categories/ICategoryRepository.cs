namespace SimpleWPFWork.Domain.Entities.Categories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetFilteredAsync(
          string? name = null,
          string? color = null,
          int page = 0,
          int limit = 100);

        Task<Category> UpdateAsync(Category category);
        Task<Category> GetAsync(Guid id);
        Task<Category> CreateAsync(Category input);
        Task DeleteAsync(Guid id);
    }
}
