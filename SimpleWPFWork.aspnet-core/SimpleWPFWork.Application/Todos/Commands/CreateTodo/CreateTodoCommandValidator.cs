using FluentValidation;
using SimpleWPFWork.ApplicationContracts.Todos.Commands.CreateTodo;

namespace SimpleWPFWork.Application.Todos.Commands.CreateTodo
{
    public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
    {
        public CreateTodoCommandValidator()
        {
            // Title zorunlu ve maksimum 200 karakter
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Todo başlığı boş olamaz")
                .MaximumLength(200)
                .WithMessage("Todo başlığı en fazla 200 karakter olabilir");

            // Description opsiyonel ama varsa max 1000 karakter
            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Açıklama en fazla 1000 karakter olabilir")
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

           


            // Username zorunlu ve maksimum 100 karakter
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Kullanıcı adı boş olamaz")
                .MaximumLength(100)
                .WithMessage("Kullanıcı adı en fazla 100 karakter olabilir");
        }
    }
}