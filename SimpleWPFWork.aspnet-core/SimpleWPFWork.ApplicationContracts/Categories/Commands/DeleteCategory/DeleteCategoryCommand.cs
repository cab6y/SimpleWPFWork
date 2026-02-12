using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleWPFWork.ApplicationContracts.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
