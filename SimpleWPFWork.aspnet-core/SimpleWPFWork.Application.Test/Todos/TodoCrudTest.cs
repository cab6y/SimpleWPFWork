using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SimpleWPFWork.Application.Test.Fixtures;
using SimpleWPFWork.ApplicationContracts.Categories.Queries.GetCategoryList;
using SimpleWPFWork.ApplicationContracts.Todos.Commands.CreateTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Commands.DeleteTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Commands.UpdateTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Queries.GetTodoList;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SimpleWPFWork.Application.Test.Todos
{
    public class TodoCrudTest : IClassFixture<TestFixture>
    {
        private readonly IMediator _mediator;

        public TodoCrudTest(TestFixture fixture)
        {
            var scope = fixture.ServiceProvider.CreateScope();
            _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        }

        [Fact]
        public async Task TodoCrudTestAsync()
        {
            // 1 kategori çek
            var categories = await _mediator.Send(new GetCategoryListQuery
            {
                Page = 0,
                Limit = 1
            });

            Assert.NotNull(categories);
            Assert.True(categories.Count > 0, "En az bir kategori olmalı");

            var firstCategory = categories[0];

            // İlk kategoriye bir todo oluştur
            var createTodo = await _mediator.Send(new CreateTodoCommand
            {
                Title = "Test Todo",
                Description = "Test Description",
                IsCompleted = false,
                Priority = "High",
                DueDate = DateTime.Now,
                CategoryId = firstCategory.Id,
                Username = "testuser"
            });

            Assert.NotNull(createTodo);
            Assert.Equal("Test Todo", createTodo.Title);

            // Todo listele
            var todoList = await _mediator.Send(new GetTodoListQuery
            {
                Page = 0,
                Limit = 10
            });

            Assert.NotNull(todoList);
            Assert.True(todoList.Count > 0);

            // Todo güncelle
            var updateTodo = await _mediator.Send(new UpdateTodoCommand
            {
                Id = createTodo.Id,
                Title = "Updated Todo",
                Description = "Updated Description",
                IsCompleted = true,
                Priority = "Normal",
                DueDate = createTodo.DueDate,
                CategoryId = firstCategory.Id,
                Username = "testuser"
            });

            Assert.Equal("Updated Todo", updateTodo.Title);
            Assert.True(updateTodo.IsCompleted);

            // Todo sil
            await _mediator.Send(new DeleteTodoCommand
            {
                Id = createTodo.Id
            });

            // Silindiğini doğrula
            var todosAfterDelete = await _mediator.Send(new GetTodoListQuery
            {
                Page = 0,
                Limit = 100
            });

            Assert.DoesNotContain(todosAfterDelete, t => t.Id == createTodo.Id);
        }
    }
}