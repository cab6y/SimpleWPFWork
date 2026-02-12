using MediatR;

namespace SimpleWPFWork.ApplicationContracts.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<CategoryDto>
    {
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
