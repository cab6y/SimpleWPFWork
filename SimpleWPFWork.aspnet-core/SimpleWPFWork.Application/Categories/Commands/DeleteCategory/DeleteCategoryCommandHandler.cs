using MediatR;
using SimpleWPFWork.ApplicationContracts.Categories.Commands.DeleteCategory;
using SimpleWPFWork.Domain.Entities.Categories;
using SimpleWPFWork.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWPFWork.Application.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly ICategoryRepository _categoryrepository;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryrepository) => _categoryrepository = categoryrepository;

        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken) => await _categoryrepository.DeleteAsync(request.Id);
    }
}