using FluentValidation;
using SimpleWPFWork.ApplicationContracts.Categories.Commands.CreateCategory;

namespace SimpleWPFWork.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Kategori adı boş olamaz")
                .MaximumLength(50).WithMessage("Kategori adı en fazla 50 karakter olabilir");

            //RuleFor(x => x.Color)
            //    .MaximumLength(7).WithMessage("Renk kodu en fazla 7 karakter olabilir (#RRGGBB)")
            //    .Matches("^#[0-9A-Fa-f]{6}$").WithMessage("Geçerli bir renk kodu girin (#RRGGBB)");
        }
    }
}