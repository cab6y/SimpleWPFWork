using FluentValidation;
using SimpleWPFWork.ApplicationContracts.Categories.Commands.UpdateCategory;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleWPFWork.Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id boş olamaz");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Kategori adı boş olamaz")
                .MaximumLength(50).WithMessage("Kategori adı en fazla 50 karakter olabilir");

            RuleFor(x => x.Color)
                .MaximumLength(7).WithMessage("Renk kodu en fazla 7 karakter olabilir")
                .Matches("^#[0-9A-Fa-f]{6}$").WithMessage("Geçerli bir renk kodu girin (#RRGGBB)");
        }
    }
}
