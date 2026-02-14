namespace SimpleWPFWork.Domain.Entities.Categories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetFilteredAsync(
          string? name = null,
          string? color = null,
          int page = 0,
          int limit = 100);
    }
}
