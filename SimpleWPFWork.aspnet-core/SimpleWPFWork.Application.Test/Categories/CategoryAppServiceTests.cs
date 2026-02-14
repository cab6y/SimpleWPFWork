using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SimpleWPFWork.Application.Test.Fixtures;
using SimpleWPFWork.ApplicationContracts.Categories.Commands.CreateCategory;
using SimpleWPFWork.ApplicationContracts.Categories.Commands.DeleteCategory;
using SimpleWPFWork.ApplicationContracts.Categories.Commands.UpdateCategory;
using SimpleWPFWork.ApplicationContracts.Categories.Queries.GetCategoryList;
using System.Threading.Tasks;
using Xunit;

namespace SimpleWPFWork.Application.Test.Categories
{
    public class CategoryAppServiceTests : IClassFixture<TestFixture>
    {
        private readonly IMediator _mediator;

        public CategoryAppServiceTests(TestFixture fixture)
        {
            var scope = fixture.ServiceProvider.CreateScope();
            _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        }

        [Fact]
        public async Task CategoryCrudTestAsync()
        {
            // CREATE
            var insert = await _mediator.Send(new CreateCategoryCommand
            {
                Name = "Test Category", 
                Color = "#FF0000" 
            });

            Assert.NotNull(insert);
            Assert.Equal("Test Category", insert.Name);
            Assert.Equal("#FF0000", insert.Color);     

            // LIST
            var list = await _mediator.Send(new GetCategoryListQuery
            {
                Page = 0,
                Limit = 10
            });

            Assert.NotNull(list);
            Assert.True(list.Count > 0);

            // UPDATE
            var updated = await _mediator.Send(new UpdateCategoryCommand
            {
                Id = insert.Id,
                Name = "Updated Category",
                Color = "#00FF00"         
            });

            Assert.Equal("Updated Category", updated.Name); 
            Assert.Equal("#00FF00", updated.Color);         

            // DELETE
            await _mediator.Send(new DeleteCategoryCommand { Id = insert.Id });

            // Verify Deleted
            var listAfter = await _mediator.Send(new GetCategoryListQuery
            {
                Page = 0,
                Limit = 10
            });

            Assert.DoesNotContain(listAfter, c => c.Id == insert.Id);
        }
    }
}