using MediatR;
using SimpleWPFWork.ApplicationContracts.Categories.Commands.DeleteCategory;
using SimpleWPFWork.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWPFWork.Application.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly SimpleWPFWorkDbContext _context;

        public DeleteCategoryCommandHandler(SimpleWPFWorkDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FindAsync(new object[] { request.Id }, cancellationToken);

            if (category == null)
                throw new Exception($"Kategori bulunamadı: {request.Id}");

            _context.Categories.Remove(category); // Soft delete (interceptor sayesinde)
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}